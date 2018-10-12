using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashManager : MonoBehaviour {

	public static CrashManager instance = null;
	// unity cant serialize dictionary, fuck
	//public Dictionary<int, Sprite> crashReasonSprite;
	public Sprite[] crashReasonSprite;
	
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
		if(force > 10.0f)
		{
			if(crashReasonSprite.Length != 0)
			{
				reason = Mathf.Min(Mathf.Max(0, reason), crashReasonSprite.Length - 1);
				GameOverManager.instance.SetCrashImage(crashReasonSprite[reason]);
				GameOverManager.instance.ShowPopup();
			}
		}
	}
	
	void Update()
	{
		
	}


}
