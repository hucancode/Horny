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
	}

	void OnEnable()
	{
		Events.instance.AddListener<SkillActivatedEvent>(OnSkillActivated);
		Events.instance.AddListener<SkillCanceledEvent>(OnSkillCanceled);
	}

	void OnDisable()
	{
		Events.instance.RemoveListener<SkillActivatedEvent>(OnSkillActivated);
		Events.instance.RemoveListener<SkillCanceledEvent>(OnSkillCanceled);
	}
	
	void OnSkillActivated(SkillActivatedEvent e)
	{
		Debug.Log("OnSkillActivated");
		if(state == State.READY)
		{
			state = State.WIND_UP;
			castTimer = 0.0f;
		}
	}

	void OnSkillCanceled(SkillCanceledEvent e)
	{
		if(state == State.WIND_UP)
		{
			state = State.READY;
		}
	}

	void FixedUpdate ()
	{
		switch(state)
		{
			case State.READY:
			break;
			case State.WIND_UP:
				castTimer += Time.fixedDeltaTime;
				if(castTimer >= castTime)
				{
					state = State.FLYING;
					movement.linearMaxSpeed *= speedBoost;
					//movement.linearSpeed = movement.linearMaxSpeed;
					movement.linearAcceleration *= speedBoost;
					controller.enabled = false;
					physics.isTrigger = true;
					life.damageReduction = 1.0f;
					durationTimer = 0.0f;
					NewTrackRequestedEvent e = new NewTrackRequestedEvent();
					Events.instance.Raise(e);
					Debug.Log("flying began");
				}
			break;
			case State.FLYING:
				durationTimer += Time.fixedDeltaTime;
				if(durationTimer > duration)
				{
					state = State.BACK_SWING;
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
				backSwingTimer += Time.fixedDeltaTime;
				if(backSwingTimer >= backSwingTime)
				{
					state = State.READY;
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
