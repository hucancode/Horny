using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerGenerator : MonoBehaviour {

	public GameObject[] seeds;
	public BoxCollider2D spawnArea;
	public float interval;
	public float linearMaxSpeed;
	public float linearMaxSpeedVar;

	private float timer;
	private const float RACER_Z = -3.0f;
	
	void Start ()
	{
		interval = 5.0f;
		linearMaxSpeed = 120.0f;
		linearMaxSpeedVar = 50.0f;
		timer = interval;
	}
	
	void FixedUpdate ()
	{
		timer -= Time.fixedDeltaTime;
		if(timer <= 0.0f)
		{
			timer = interval;
			SpawnRacerRandom();
		}
	}

	public void SpawnRacerRandom()
	{
		if(seeds.Length == 0)
		{
			return;
		}
		int i = (int)Random.Range(0.0f, seeds.Length);
		if(i == seeds.Length)
		{
			i--;
		}
		Vector3 position = Vector3.zero;
		float half_w = spawnArea.size.x/2;
		float half_h = spawnArea.size.y/2;
		position.x = transform.position.x + Random.Range(-half_w, half_w);
		position.y = transform.position.y + Random.Range(-half_h, half_h);
		position.z = RACER_Z;
		GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
		RacerMovement movement_component = clone.GetComponent<RacerMovement>();
		movement_component.linearMaxSpeed = Random.Range(
			linearMaxSpeed - linearMaxSpeedVar, linearMaxSpeed + linearMaxSpeedVar);
		movement_component.enableSwing = (Random.Range(0.0f, 1.0f) > 0.5f);
		// TODO: implement a pool that actually is a pool
		RacerPool.instance.Push(clone);
	}
}
