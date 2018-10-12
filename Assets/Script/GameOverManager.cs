using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	enum State
	{
		FADING_IN,
		EXP_GAINED,
		RETRY_PROMPT,
		FADING_OUT,
		FADED_OUT
	}
	public static GameOverManager instance = null;
	public Image crashImage;
	public Animator animator;
	public GameObject expGauge;
	public GameObject retryPrompt;

	private State state;
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
	
	void Start()
	{
		state = State.FADING_IN;
	}
	public void SetCrashImage(Sprite sprite)
	{
		crashImage.sprite = sprite;
	}
	public void SetExpGauge(int exp)
	{
		
	}

	public void ShowPopup()
	{
		state = State.FADING_IN;
	}

	void Update ()
	{
		
	}
}
