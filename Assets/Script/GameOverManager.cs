using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	public static GameOverManager instance = null;
	public GameOverPopup gameOverPopup;
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		if(instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public void GameOver(int reason)
	{
		gameOverPopup.SetCrashReason(reason);
		// stop game logic
	}
	public void GameRestart()
	{
		
	}
}
