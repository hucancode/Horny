using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerNPCController : RacerController {

	public CapsuleCollider2D sightChecker;

	enum State
	{
		Moving,
		Thinking,
		Solving
	}

	private State state;
	private float timeSinceStateChange;
	private float currentHitTestAngle;
	private float hitTestAngleMax;
	private int leftHitTestResult;
	private int rightHitTestResult;

	private float HIT_TEST_PRECISION = 10.0f;
	private float REACTION_TIME = 0.8f;

	void Start ()
	{
		state = State.Moving;
		timeSinceStateChange = 0.0f;
	}
	
	void ThinkBegin()
	{
		timeSinceStateChange = 0.0f;
		state = State.Thinking;
		movementComponent.BrakeBegin();
		currentHitTestAngle = -movementComponent.turnRange;
		hitTestAngleMax = movementComponent.turnRange;
	}
	void ThinkUpdate()
	{
		if(currentHitTestAngle < hitTestAngleMax)
		{
			Quaternion target = Quaternion.Euler(0, 0, currentHitTestAngle);
			sightChecker.gameObject.transform.rotation = (target);
			Collider2D[] result = new Collider2D[20];
			ContactFilter2D filter = new ContactFilter2D();
			int hitCount = sightChecker.OverlapCollider(filter, result);
			if(currentHitTestAngle < 0)
			{
				rightHitTestResult += hitCount;
			}
			else if(currentHitTestAngle > 0)
			{
				leftHitTestResult += hitCount;
			}
			currentHitTestAngle += HIT_TEST_PRECISION;
		}
		else
		{
			movementComponent.BrakeEnd();
			SolveBegin();
		}
	}

	void SolveBegin()
	{
		bool to_the_left = leftHitTestResult < rightHitTestResult;
		movementComponent.TurnBegin(to_the_left);
		Quaternion target = Quaternion.Euler(0, 0, 0);
		sightChecker.gameObject.transform.rotation = target;
		timeSinceStateChange = 0.0f;
		state = State.Solving;
	}
	
	void SolveUpdate()
	{
		if(timeSinceStateChange > REACTION_TIME)
		{
			movementComponent.TurnEnd();
			MoveBegin();
		}
	}

	void MoveBegin()
	{
		timeSinceStateChange = 0.0f;
		state = State.Moving;
	}

	void MoveUpdate()
	{
		if(timeSinceStateChange > REACTION_TIME)
		{
			Collider2D[] result = new Collider2D[20];
			ContactFilter2D filter = new ContactFilter2D();
			int hitCount = sightChecker.OverlapCollider(filter, result);
			if(hitCount != 0)
			{
				ThinkBegin();
			}
			timeSinceStateChange = 0.0f;
		}
	}

	void Update ()
	{
		timeSinceStateChange += Time.deltaTime;
		switch(state)
		{
			case State.Moving:
				MoveUpdate();
			break;
			case State.Thinking:
				ThinkUpdate();
			break;
			case State.Solving:
				SolveUpdate();
			break;
		}
	}
}
