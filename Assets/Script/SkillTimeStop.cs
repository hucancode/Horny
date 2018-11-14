using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTimeStop : MonoBehaviour {

	public enum State
	{
		READY,
		FLYING
	}
	public RacerMovement movement;
	public RacerLife life;
	public RacerHumanController controller;
	public BoxCollider2D physics;

	public float duration;
	public float durationTimer;
	public float speedBoost;
	public float fps;
	public float physicsFps;
	public State state;

	void Start()
	{
		state = State.READY;
	}

	void OnEnable()
	{
		Events.instance.AddListener<SkillActivatedEvent>(OnSkillActivated);
	}

	void OnDisable()
	{
		Events.instance.RemoveListener<SkillActivatedEvent>(OnSkillActivated);
	}
	
	void OnSkillActivated(SkillActivatedEvent e)
	{
		Debug.Log("OnSkillActivated");
		if(state == State.READY)
		{
			state = State.FLYING;
			float speedBoost2 = speedBoost*speedBoost;
			movement.linearMaxSpeed *= speedBoost2;
			movement.hypebolicAccelerateTime /= speedBoost;
			movement.angularSpeed *= speedBoost;
			Time.timeScale = 1.0f/speedBoost;
			Time.fixedDeltaTime /= speedBoost;
			durationTimer = 0.0f;
			Debug.Log("flying began");
		}
	}

	void FixedUpdate ()
	{
		fps = 1.0f/Time.unscaledDeltaTime;
		physicsFps = 1.0f/Time.fixedUnscaledDeltaTime;
		switch(state)
		{
			case State.READY:
			break;
			case State.FLYING:
				durationTimer += Time.fixedUnscaledDeltaTime;
				if(durationTimer > duration)
				{
					state = State.READY;
					float speedBoost2 = speedBoost*speedBoost;
					movement.linearMaxSpeed /= speedBoost2;
					movement.hypebolicAccelerateTime *= speedBoost;
					movement.angularSpeed /= speedBoost;
					Time.timeScale = 1.0f;
					Time.fixedDeltaTime *= speedBoost;
					movement.ResetRotationAndVelocity();
					Debug.Log("flying ended");
				}
			break;
			default:
			break;
		}
	}
}
