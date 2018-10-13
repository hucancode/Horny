using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MonoBehaviour {

	public GameOverPopup parent;
	private GameObject canvas;
	// Use this for initialization
	void Start () {
		canvas = GameObject.FindGameObjectWithTag("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Press()
	{
		parent.FadeOut();
		DontDestroyOnLoad(canvas);
		GameManager.instance.GameRestart();
		StartCoroutine(WaitAndDestroy());
	}
	public IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSecondsRealtime(1.2f);
		Destroy(parent.gameObject);
	}
}
