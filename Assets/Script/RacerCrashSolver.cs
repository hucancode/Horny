using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerCrashSolver : MonoBehaviour {

	public int crashReasonKey;
	
	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
		if(collision.transform.gameObject != GameManager.instance.mainCharacter)
		{
			return;
		}
		
        CrashManager.instance.Crash(crashReasonKey, collision.relativeVelocity.magnitude);
    }
}
