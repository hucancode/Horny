﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerManager.instance.Spawn();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}