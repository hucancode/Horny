using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpritePicker : MonoBehaviour {

	public Sprite[] spites;
	public SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake () {
		int i = (int)Random.Range(0.0f, spites.Length);
		if(i == spites.Length)
		{
			i--;
		}
		spriteRenderer.sprite = spites[i];
	}
}
