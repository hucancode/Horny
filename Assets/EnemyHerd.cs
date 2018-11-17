using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampedeActivatedEvent: GameEvent
{

}
public class EnemyHerd : MonoBehaviour {

	public RacerMovement movement;
	bool isInStampede;
	float stampedeTimer;
	const float stampedeTime = 2.5f;
	const float speedBoost = 10.0f;
	void Start ()
	{
		isInStampede = false;
	}

	void OnEnable()
	{
		Events.instance.AddListener<StampedeActivatedEvent>(OnStampedeActivated);
	}

	void OnDisable()
	{
		Events.instance.RemoveListener<StampedeActivatedEvent>(OnStampedeActivated);
	}

	void FixedUpdate()
	{
		if(isInStampede)
		{
			stampedeTimer += Time.fixedDeltaTime;
			if(stampedeTimer > stampedeTime)
			{
				isInStampede = false;
				movement.linearMaxSpeed /= speedBoost;
				movement.linearAcceleration /= speedBoost;
				movement.ResetRotationAndVelocity();
				Debug.Log("stampede ended");
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		StampedeActivatedEvent e = new StampedeActivatedEvent();
		Events.instance.Raise(e);
	}

	void OnStampedeActivated(StampedeActivatedEvent e)
	{
		Debug.Log("OnStampedeActivated");
		if(isInStampede)
		{
			return;
		}
		isInStampede = true;
		stampedeTimer = 0.0f;
		movement.linearMaxSpeed *= speedBoost;
		movement.linearAcceleration *= speedBoost;
	}
}
