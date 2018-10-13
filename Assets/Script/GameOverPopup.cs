using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour {

	// unity cant serialize dictionary, fuck
	//public Dictionary<int, Sprite> crashReasonSprite;
	public Sprite[] crashReasonSprite;
	public Image crashImage;
	public Image overlay;
	public Slider expGauge;
	public GameObject retryPrompt;
	public Animator crashImageAnimator;
	public Animator overlayAnimator;
	public Animator retryPromptAnimator;

	void Start()
	{
	}

	public void SetCrashReason(int reason)
	{
		reason = (int)Random.Range(0.0f, crashReasonSprite.Length);
		reason = Mathf.Min(Mathf.Max(0, reason), crashReasonSprite.Length - 1);
		crashImage.sprite = crashReasonSprite[reason];
	}

	public void SetExpGauge(float percent)
	{
		expGauge.value = percent;
	}

	public void FadeOut()
	{
		crashImageAnimator.SetBool("FadingIn", false);
		overlayAnimator.SetBool("FadingIn", false);
		retryPromptAnimator.SetBool("FadingIn", false);
	}

	void Update ()
	{
		
	}
}
