using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MonoBehaviour {

	public GameOverPopup parent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Press()
	{
		parent.FadeOut();
		StartCoroutine(WaitAndRestartGame());
	}
	public IEnumerator WaitAndRestartGame()
	{
		yield return new WaitForSecondsRealtime(1.0f);
		GameManager.instance.GameRestart();
	}
}
