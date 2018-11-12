using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBallLightning : RacerSkillBase
{
	public enum State
	{
		READY,
		WIND_UP,
		FLYING,
		BACK_SWING
	}

	public RacerMovement movement;
	public RacerLife life;
	public RacerHumanController controller;
	public BoxCollider2D physics;

	public float duration;
	public float durationTimer;
	public float backSwingTime;
	public float backSwingTimer;
	public State state;
	public float speedBoost;

	void Start()
	{
		state = State.READY;
		Events.instance.AddListener<SkillActivatedEvent>(OnSkillActivated);
		Events.instance.AddListener<SkillCanceledEvent>(OnSkillCanceled);
	}
	
	void OnSkillActivated(SkillActivatedEvent e)
	{
		Debug.Log("OnSkillActivated");
		if(state == State.READY)
		{
			state = State.WIND_UP;
			castTimer = 0.0f;
			// switch to windup animation
		}
	}

	void OnSkillCanceled(SkillCanceledEvent e)
	{
		if(state == State.WIND_UP)
		{
			state = State.READY;
			// return to ready animation
		}
	}

	void Update ()
	{
		switch(state)
		{
			case State.READY:
			break;
			case State.WIND_UP:
				castTimer += Time.deltaTime;
				if(castTimer >= castTime)
				{
					state = State.FLYING;
					// switch to flying animation
					// enable particle effect
					// set player to invulnerable
					// set player speed to x10
					movement.linearMaxSpeed *= speedBoost;
					//movement.linearSpeed = movement.linearMaxSpeed;
					movement.linearAcceleration *= speedBoost;
					controller.enabled = false;
					physics.isTrigger = true;
					life.damageReduction = 1.0f;
					durationTimer = 0.0f;
					Debug.Log("flying began");
				}
			break;
			case State.FLYING:
				durationTimer += Time.deltaTime;
				if(durationTimer > duration)
				{
					state = State.BACK_SWING;
					// return to ready animation
					// disable particle effect
					// set player to vulnerable
					// set player speed to x0.1
					movement.linearMaxSpeed /= speedBoost;
					movement.linearAcceleration /= speedBoost;
					//movement.linearSpeed = movement.linearMaxSpeed;
					controller.enabled = true;
					physics.isTrigger = false;
					life.damageReduction = 0.0f;
					backSwingTimer = 0.0f;
					movement.ResetRotationAndVelocity();
				}
			break;
			case State.BACK_SWING:
				backSwingTimer += Time.deltaTime;
				if(backSwingTimer >= backSwingTime)
				{
					state = State.READY;
					// switch to flying animation
					// enable particle effect
					// set player to invulnerable
					// set player speed to x10
					//movement.linearSpeed = movement.linearMaxSpeed;
					movement.ResetRotationAndVelocity();
					Debug.Log("flying ended");
				}
			break;
			default:
			break;
		}
	}
}
