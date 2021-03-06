﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPool : MonoBehaviour {

	public static TrackPool instance = null;
	private Queue<GameObject> pool;
	public int poolSize;
	
	void Awake()
	{
		poolSize = 5;
		if(instance == null)
		{
			instance = this;
		}
		if(instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		pool = new Queue<GameObject>();
	}

	public void Push(GameObject game_object)
	{
		pool.Enqueue(game_object);
		while(pool.Count > poolSize)
		{
			Debug.Log("pool too big, release old objects");
			GameObject old_object = pool.Dequeue();
			Destroy(old_object);
		}
	}
}
