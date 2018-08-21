using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPasserMovement : MonoBehaviour {

	[Tooltip("Rigidbody2D component")]
	public Rigidbody2D rigidBody;
	[Tooltip("m/s, higher value means faster travel.")]
	public float linearMaxSpeed;
	[Tooltip("m/s, lower value means more brake effective.")]
	public float linearMinSpeed;
	[Tooltip("m/s^2, higher value means faster brake.")]
	public float linearAcceleration;
	//[HideInInspector]
	public float linearSpeed;// m/s
	[Tooltip("Degree, where to look at")]
	public float lookAtAngle;// degree

	private const float SPRITE_ANGLE = 0.0f;
	private const float KILL_X = 100.0f;
	
	void Update ()
	{
		if(rigidBody == null)
		{
			return;
		}

		if(transform.position.x < -KILL_X || transform.position.x > KILL_X)
		{
			Destroy(gameObject);
		}

		float micro_acc = linearAcceleration * Time.fixedDeltaTime;
		linearSpeed += micro_acc;
		linearSpeed = Mathf.Clamp(linearSpeed, linearMinSpeed, linearMaxSpeed);
		float micro_speed = linearSpeed * Time.fixedDeltaTime;
		Vector2 thrust = Vector2.right * micro_speed;
		thrust = Quaternion.Euler(0.0f, 0.0f, lookAtAngle) * thrust;
		rigidBody.AddForce(thrust);
		SetRigidBodyRotation(lookAtAngle);
	}

	void SetRigidBodyRotation(float new_rotation)
	{
		rigidBody.MoveRotation(new_rotation - SPRITE_ANGLE);
	}
}
