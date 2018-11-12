using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerCrashSolver : MonoBehaviour {

	public int vehicleKey;
	
	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
		GameObject main = GameManager.instance.mainCharacter;
		if(collision.transform.gameObject != main)
		{
			return;
		}
		RacerLife lifeComponent = main.GetComponent<RacerLife>();
		lifeComponent.ReceiveDamage(vehicleKey, collision.relativeVelocity.magnitude);
    }
}
