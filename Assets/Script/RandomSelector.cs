using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : MonoBehaviour {

	public MonoBehaviour[] components;
	// Use this for initialization
	void Start () 
	{
		foreach(MonoBehaviour item in components)
		{
			item.enabled = false;
		}
		int i = (int)Random.Range(0.0f, components.Length);
		if(i == components.Length)
		{
			i--;
		}
		components[i].enabled = true;
	}
}
