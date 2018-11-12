using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerLife : MonoBehaviour {

	public float health;
	public float damageReduction;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	public void ReceiveDamage(int vehicleKey, float damage)
	{
		Debug.Log("before received damage "+ damage);
		damage *= 1.0f - damageReduction;
		Debug.Log("received damage "+ damage);
		health -= damage;
		if(health <= 0.0f)
		{
			//GameManager.instance.GameOver(vehicleKey);
		}
	}
}
