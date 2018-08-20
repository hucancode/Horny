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
		DYNAMIC_CHARGE = 0,
		CONSTANT_CHARGE = 1
	};

	[Tooltip("Rigidbody2D component")]
	public Rigidbody2D rigidBody;
	[Tooltip("m/s, higher value means faster travel.")]
	public float linearMaxSpeed;
	[Tooltip("m/s, lower value means more brake effective.")]
	public float linearMinSpeed;
	[Tooltip("m/s^2, higher value means faster brake.")]
	public float linearAcceleration;
	[Tooltip("Degree/s, higher value means faster rotation.")]
	public float angularSpeed;
	[Tooltip("Degree, higher value means wider rotation range.")]
	public float turnRange;
	[Tooltip("Racer behavior")]
	public RacerBehavior behavior;
	[Tooltip("Enable swing physics")]
	public bool enableSwing = true;
	//[HideInInspector]
	public RacerState state;
	//[HideInInspector]
	public float linearSpeed;// m/s
	
	private float targetRotation;// degree
	private Vector2 swingPivot;

	private const float INITIAL_ANGLE = 90.0f;
	private const float VISION_LENGTH = 4.0f;
	private const float ROTATION_TOLERANCE = 5.0f;
	private const float SWING_POWER = 0.3f; // 0.0 = no swing, 1.0 = swing forever, never stop
	private const bool DYNAMIC_ROTATION = true;
	private const float DYNAMIC_ROTATION_FORCE = 5.0f;

	void Start() 
	{
		state = RacerState.CHARGING;
		swingPivot = Vector2.zero;
		targetRotation = INITIAL_ANGLE;
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
		float d = targetRotation - GetRigidBodyRotation();
		if(Mathf.Abs(d) > ROTATION_TOLERANCE)
		{
			float new_rotation = GetRigidBodyRotation() + Mathf.Sign(d)*angularSpeed*Time.fixedDeltaTime;
			SetRigidBodyRotation(new_rotation);
			{
				Vector3 r_vector = new Vector3(Mathf.Cos((targetRotation)* Mathf.Deg2Rad), Mathf.Sin((targetRotation)* Mathf.Deg2Rad));
				Debug.DrawLine(transform.position, transform.position+Vector3.Normalize(r_vector)*VISION_LENGTH, Color.red);
			}
		}
		else
		{
			SetRigidBodyRotation(targetRotation);
		}
	}

	void UpdateMovement()
	{
		if(rigidBody == null)
		{
			return;
		}
		
		float micro_acc = linearAcceleration * Time.fixedDeltaTime;
		linearSpeed += micro_acc;
		linearSpeed = Mathf.Clamp(linearSpeed, linearMinSpeed, linearMaxSpeed);
		float micro_speed = linearSpeed * Time.fixedDeltaTime;
		Vector2 thrust = Vector2.right * micro_speed;
		thrust = Quaternion.Euler(0.0f, 0.0f, GetRigidBodyRotation()) * thrust;
		rigidBody.AddForce(thrust);
		
		if(DYNAMIC_ROTATION)
		{
			CompensateRotation();
		}
		{
			Vector3 r_vector = new Vector3(thrust.x, thrust.y);
			Debug.DrawLine(transform.position, transform.position+Vector3.Normalize(r_vector)*VISION_LENGTH, Color.green);
		}
	}

	void CompensateRotation()
	{
		float micro_speed = linearSpeed * Time.fixedDeltaTime;
		float current_rotation = Vector2.Angle(rigidBody.velocity, Vector2.right);
		float r = Mathf.Abs(targetRotation - current_rotation)/180.0f*DYNAMIC_ROTATION_FORCE;
		Vector2 lean = Vector2.right * micro_speed * r * r;
		lean = Quaternion.Euler(0.0f, 0.0f, GetRigidBodyRotation()) * lean;
		rigidBody.AddForce(lean);
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

		swingPivot.y = transform.position.y + VISION_LENGTH*SWING_POWER;

		float target_angle = Vector2.Angle(swingPivot - position_2d, Vector2.right);
		targetRotation = target_angle;
		{
			Vector3 r_vector = new Vector3(swingPivot.x, swingPivot.y);
			Debug.DrawLine(transform.position, r_vector, Color.blue);
		}
		Debug.Log("swingging..., target angle = " + targetRotation);
	}

	public void TurnBegin(bool is_left)
	{
		targetRotation = (is_left?turnRange:-turnRange) + INITIAL_ANGLE;
		state = RacerState.TURNING;
		Debug.Log("Turn begin " + targetRotation);
	}

	public void TurnUpdate(bool is_left)
	{
		targetRotation = (is_left?turnRange:-turnRange) + INITIAL_ANGLE;
	}

	public void TurnEnd()
	{
		targetRotation = INITIAL_ANGLE;
		swingPivot = transform.position;
		state = RacerState.CHARGING;
		if(!enableSwing)
		{
			ResetRotation();
		}
	}

	public void ResetRotation()
	{
		targetRotation = INITIAL_ANGLE;
		Vector2 new_velocity = Vector2.right * rigidBody.velocity.magnitude;
		new_velocity = Quaternion.Euler(0.0f, 0.0f, targetRotation) * new_velocity;
		rigidBody.velocity = new_velocity;
	}

	public void ResetRotationAndVelocity()
	{
		rigidBody.rotation = 0.0f;
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
	
	float GetRigidBodyRotation()
	{
		return rigidBody.rotation + INITIAL_ANGLE;
	}

	void SetRigidBodyRotation(float new_rotation)
	{
		rigidBody.MoveRotation(new_rotation - INITIAL_ANGLE);
	}
}
