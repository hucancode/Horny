using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseTimerUpdate : MonoBehaviour {

	public Text displayText;
	
	void Start () {
		
	}
	
	void Update ()
	{
		if(GameManager.instance.state == GameManager.State.Pausing)
		{
			displayText.enabled = true;
			displayText.text = "Paused";
		}
		else if(GameManager.instance.state == GameManager.State.ToRunning)
		{
			displayText.enabled = true;
			displayText.text = ((int)GameManager.instance.pauseCountDownTimer).ToString();;
		}
		else
		{
			displayText.enabled = false;
		}
	}
}
