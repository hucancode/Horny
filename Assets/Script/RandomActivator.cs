using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActivator : MonoBehaviour {

	public MonoBehaviour component;
	public float hitRate;
	// Use this for initialization
	void Awake () {
		component.enabled = false;
		float r = Random.Range(0.0f, 100.0f);
		if(r <= hitRate)
		{
			component.enabled = true;
		}
	}
}
