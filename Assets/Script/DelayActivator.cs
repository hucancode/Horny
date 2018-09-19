using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActivator : MonoBehaviour {

	public MonoBehaviour component;
	public float delayTime;
	public float accumulatedTime;
	// Use this for initialization
	void Start ()
	{
		accumulatedTime = 0.0f;
		component.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(component.enabled)
		{
			return;
		}
		accumulatedTime += Time.fixedDeltaTime;
		if(accumulatedTime > delayTime)
		{
			component.enabled = true;
		}
	}
}
