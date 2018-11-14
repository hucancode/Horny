using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RacerMovement : MonoBehaviour {

	public enum RacerState
	{
		CHARGING = 0,
		TURNING = 1
	};

	public enum RacerBehavior
	{
		CHARGE_90 = 0,
		CHARGE_270 = 1
	};

	public enum AccelerationFunction
	{
		LINEAR,
		HYPERBOLIC
	}

	[Tooltip("Rigidbody2D component")]
	public Rigidbody2D rigidBody;
	[Tooltip("m/s, higher value means faster travel.")]
	public float linearMaxSpeed;
	[Tooltip("m/s, lower value means more brake effective.")]
	public float linearMinSpeed;
	[Tooltip("m/s^2, higher value means faster brake.")]
	public float linearAcceleration;
	[Tooltip("time to accelerate from min to max")]
	public float hypebolicAccelerateTime;
	[Tooltip("acceleration function")]
	public AccelerationFunction acceleration;
	[Tooltip("Degree/s, higher value means faster rotation.")]
	public float angularSpeed;
	[Tooltip("Degree, higher value means wider rotation range.")]
	public float turnRange;
	[Tooltip("Racer behavior")]
	public RacerBehavior behavior;
	[Tooltip("Enable swing physics")]
	public bool enableSwing = true;
	[Tooltip("How strong racer should swing, higher value mean less swing")]
	public float swingPower = 0.3f;
	[Tooltip("Enable dynamic turning, maintaining y-velocity while turning")]
	public const bool dynamicTurning = true;
	[Tooltip("Skip force calculation. For object that heavily rely on physics, use this can overcome physics engine limitation")]
	public bool useImmediateForce = false;
	//[HideInInspector]
	public RacerState state;
	//[HideInInspector]
	public float linearSpeed;// m/s
	
	private float targetAngle;// degree
	private Vector2 swingPivot;

	private const float SPRITE_ANGLE = 90.0f;
	private const float VISION_LENGTH = 4.0f;
	private const float ROTATION_TOLERANCE = 5.0f;
	
	void Start() 
	{
		state = RacerState.CHARGING;
		swingPivot = Vector2.zero;
		targetAngle = SPRITE_ANGLE;
		swingPivot = transform.position;
	}

	void FixedUpdate()
	{
		UpdateMovement();
		UpdateRotation();
		UpdateBackSwing();
	}

	void UpdateRotation()
	{
		if(rigidBody == null)
		{
			return;
		}
		float d = targetAngle - GetRigidBodyRotation();
		if(Mathf.Abs(d) > ROTATION_TOLERANCE)
		{
			float new_rotation = 0.0f;
			float micro_angle = angularSpeed*Time.fixedDeltaTime;
			if(micro_angle > Mathf.Abs(d))
			{
				new_rotation = targetAngle;
			}
			else
			{
				new_rotation = GetRigidBodyRotation() + Mathf.Sign(d)*micro_angle;
			}
			SetRigidBodyRotation(new_rotation);
			{
				Vector3 r_vector = new Vector3(Mathf.Cos((targetAngle)* Mathf.Deg2Rad), Mathf.Sin((targetAngle)* Mathf.Deg2Rad));
				Debug.DrawLine(transform.position, transform.position+Vector3.Normalize(r_vector)*VISION_LENGTH, Color.red);
			}
		}
		else
		{
			SetRigidBodyRotation(targetAngle);
		}
	}

	void UpdateMovement()
	{
		if(rigidBody == null)
		{
			return;
		}
		
		float micro_acc = 0.0f;
		switch(acceleration)
		{
			case AccelerationFunction.HYPERBOLIC:
				// the formular is: y = ax^2, where y = desired speed, x = time passed
				float a, x, y;
				y = linearMaxSpeed - linearMinSpeed;
				x = hypebolicAccelerateTime;
				a = y/(x*x);
				float x0, y0, x1, y1;
				y0 = linearSpeed - linearMinSpeed;
				y0 = Mathf.Clamp(y0, 0, y0);
				x0 = Mathf.Sqrt(y0/a);
				x1 = x0 + Time.fixedDeltaTime;
				y1 = a*x1*x1;
				micro_acc = y1 - y0;
				break;
			case AccelerationFunction.LINEAR:
				micro_acc = linearAcceleration * Time.fixedDeltaTime;
				break;
			default:
				break;
		}
		
		linearSpeed += micro_acc;
		linearSpeed = Mathf.Clamp(linearSpeed, linearMinSpeed*GameManager.instance.difficulty, linearMaxSpeed*GameManager.instance.difficulty);
		float micro_speed = linearSpeed * Time.fixedDeltaTime;
		Vector2 thrust = Vector2.right * micro_speed;
		thrust = Quaternion.Euler(0.0f, 0.0f, GetRigidBodyRotation()) * thrust;

		if(useImmediateForce)
		{
			rigidBody.velocity = thrust;
		}
		else
		{
			rigidBody.AddForce(thrust);
		}

		if(dynamicTurning)
		{
			AddSupportForce(thrust);
		}
		{
			Vector3 r_vector = new Vector3(thrust.x, thrust.y);
			Debug.DrawLine(transform.position, transform.position+Vector3.Normalize(r_vector)*VISION_LENGTH, Color.green);
		}
		Vector3 position = gameObject.transform.position;
		position.z = position.y % 100.0f;
		gameObject.transform.position = position;
	}

	void AddSupportForce(Vector2 thrust)
	{
		float ratio = 1.0f/Mathf.Cos(rigidBody.rotation*Mathf.Deg2Rad);
		Vector2 full_force = thrust*ratio;
		Vector2 support_force = full_force - thrust;
		rigidBody.AddForce(support_force);
	}

	void UpdateBackSwing()
	{
		if(!enableSwing)
		{
			return;
		}
		if(rigidBody == null)
		{
			return;
		}
		if(state != RacerState.CHARGING)
		{
			return;
		}
		Vector2 position_2d = new Vector2(transform.position.x, transform.position.y);

		swingPivot.y = transform.position.y + VISION_LENGTH*swingPower;

		float target_angle = Vector2.Angle(swingPivot - position_2d, Vector2.right);
		targetAngle = target_angle;
		{
			Vector3 r_vector = new Vector3(swingPivot.x, swingPivot.y);
			Debug.DrawLine(transform.position, r_vector, Color.blue);
		}
		//Debug.Log("swingging..., target angle = " + targetAngle);
	}

	public void TurnBegin(bool is_left)
	{
		targetAngle = (is_left?turnRange:-turnRange) + GetLookAtAngle();
		state = RacerState.TURNING;
		//Debug.Log("Turn begin " + targetAngle);
	}

	public void TurnUpdate(bool is_left)
	{
		targetAngle = (is_left?turnRange:-turnRange) + GetLookAtAngle();
	}

	public void TurnEnd()
	{
		targetAngle = GetLookAtAngle();
		swingPivot = transform.position;
		state = RacerState.CHARGING;
		if(!enableSwing)
		{
			ResetRotation();
		}
	}

	public void ResetRotation()
	{
		targetAngle = GetLookAtAngle();
		Vector2 new_velocity = Vector2.right * rigidBody.velocity.magnitude;
		new_velocity = Quaternion.Euler(0.0f, 0.0f, targetAngle) * new_velocity;
		rigidBody.velocity = new_velocity;
	}

	public void ResetRotationAndVelocity()
	{
		rigidBody.rotation = GetLookAtAngle() - SPRITE_ANGLE;
		rigidBody.velocity = Vector2.zero;
	}

	public void BrakeBegin()
	{
		linearAcceleration = -Mathf.Abs(linearAcceleration);
	}

	public void BrakeEnd()
	{
		linearAcceleration = Mathf.Abs(linearAcceleration);
	}

	public bool IsBraking()
	{
		return linearAcceleration < 0.0f;
	}
	
	public float GetRigidBodyRotation()
	{
		return rigidBody.rotation + SPRITE_ANGLE;
	}

	public float GetLookAtAngle()
	{
		return (behavior == RacerBehavior.CHARGE_90)?90.0f:270.0f;
	}

	void SetRigidBodyRotation(float new_rotation)
	{
		rigidBody.MoveRotation(new_rotation - SPRITE_ANGLE);
	}
}
