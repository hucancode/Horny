using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacerHumanController : RacerController {

	public Text swingOnOffText;

	void Start () 
	{
		GameManager.instance.mainCharacter = gameObject;
	}
	void ToggleSwingPhysics()
	{
		if(Input.GetKey("space"))
		{
			movementComponent.enableSwing = !movementComponent.enableSwing;
			swingOnOffText.text = "Swing Physics: " + (movementComponent.enableSwing?"On":"Off") + " (Press space to toggle)";
		}
	}
	void Update ()
	{
		bool has_input = false;
		Vector2 touch_position = Vector2.zero;
		TouchPhase touch_phase = TouchPhase.Began;

		// phone input
		if (Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);
			touch_position = touch.position;
			touch_phase = touch.phase;
			has_input = true;
		}

		// pc input
		if(Input.GetMouseButtonDown(0))
		{
			touch_position = Input.mousePosition;
			touch_phase = TouchPhase.Began;
			has_input = true;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			touch_position = Input.mousePosition;
			touch_phase = TouchPhase.Ended;
			has_input = true;
		}
		else if(Input.GetMouseButton(0))
		{
			touch_position = Input.mousePosition;
			touch_phase = TouchPhase.Moved;
			has_input = true;
		}
		ToggleSwingPhysics();

		// send command
		if (!has_input)
		{
			return;
		}
		if(touch_phase == TouchPhase.Began)
		{
			Vector3 racer_position = Camera.main.WorldToScreenPoint(transform.position);
			bool to_the_left = touch_position.x < racer_position.x;
			bool to_the_bottom = touch_position.y < racer_position.y;
			Debug.Log("touch began, touch_position ="+touch_position.ToString() + " racer_position ="+racer_position.ToString());
			if(to_the_bottom)
			{
				movementComponent.BrakeBegin();
			}
			{
				movementComponent.TurnBegin(to_the_left);
			}
		}
		else if(touch_phase == TouchPhase.Moved)
		{
			Vector3 racer_position = Camera.main.WorldToScreenPoint(transform.position);
			bool to_the_left = touch_position.x < racer_position.x;
			bool to_the_bottom = touch_position.y < racer_position.y;
			Debug.Log("touch moved, touch_position ="+touch_position.ToString() + " racer_position ="+racer_position.ToString());
			if(to_the_bottom && !movementComponent.IsBraking())
			{
				movementComponent.BrakeBegin();
			}
			else if(!to_the_bottom && movementComponent.IsBraking())
			{
				movementComponent.BrakeEnd();
			}
			{
				Debug.Log("start turning");
				movementComponent.TurnUpdate(to_the_left);
			}
		}
		else if(touch_phase == TouchPhase.Ended)
		{
			Debug.Log("touch ended");
			if(movementComponent.IsBraking())
			{
				Debug.Log("stop braking");
				movementComponent.BrakeEnd();
			}
			{
				Debug.Log("stop turning");
				movementComponent.TurnEnd();
			}
		}
	}
}
