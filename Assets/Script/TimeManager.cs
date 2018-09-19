using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	public enum State
	{
		Running,
		Pausing,
		ToRunning
	}

	public float countDownTime;
	public Text displayText;
	private float countDownTimer;
	private State state;

	void Start () 
	{
		countDownTimer = 0.0f;
		state = State.Running;
	}
	
	void Update ()
	{
		switch(state)
		{
			case State.Running:
				if(Input.GetButtonDown("Pause"))
				{
					Pause();
				}
			break;
			case State.Pausing:
				if(Input.GetButtonDown("Pause"))
				{
					UnpauseBegin();
				}
			break;
			case State.ToRunning:
				countDownTimer -= Time.unscaledDeltaTime;
				displayText.text = ((int)countDownTimer).ToString();
				if(countDownTimer <= 1.0f)
				{
					UnpausedEnd();
				}
			break;
		}
	}

	public void Pause()
	{
		state = State.Pausing;
		Time.timeScale = 0.0f;
		displayText.text = "Paused";
		displayText.enabled = true;
	}

	public void UnpauseBegin()
	{
		state = State.ToRunning;
		countDownTimer = countDownTime;
	}

	public void UnpausedEnd()
	{
		state = State.Running;
		Time.timeScale = 1.0f;
		displayText.enabled = false;
	}
}
