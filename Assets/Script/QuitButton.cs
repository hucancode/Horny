using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour {

	public GameOverPopup parent;
	private GameObject canvas;
	// Use this for initialization
	void Start ()
	{
		canvas = GameObject.FindGameObjectWithTag("Canvas");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Press()
	{
		parent.FadeOut();
		StartCoroutine(WaitAndDestroy());
	}
	public IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSecondsRealtime(1.2f);
		GameManager.instance.ToMainMenu();
		Destroy(parent.gameObject);
		Destroy(canvas);
	}
}
