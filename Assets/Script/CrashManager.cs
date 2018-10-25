using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashManager : MonoBehaviour {

	public static CrashManager instance = null;
    public float heavyCrashForce = 10.0f;
	
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

	public void Crash(int reason, float force)
	{
		Debug.Log("crash, reason = "+reason+"force = "+force);
        if (force > heavyCrashForce*GameManager.instance.difficulty)
		{
			GameManager.instance.GameOver(reason);
		}
	}
}
