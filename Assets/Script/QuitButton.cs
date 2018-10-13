using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour {

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
		StartCoroutine(WaitAndToMainMenu());
	}
	public IEnumerator WaitAndToMainMenu()
	{
		yield return new WaitForSecondsRealtime(1.0f);
		GameManager.instance.ToMainMenu();
	}
}
