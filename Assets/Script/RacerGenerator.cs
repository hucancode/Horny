using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerGenerator : MonoBehaviour {

	public GameObject seed;
	public BoxCollider2D spawnArea;
	public float interval = 5.0f;
	public float linearMaxSpeed = 120.0f;
	public float linearMaxSpeedVar = 50.0f;

	private float timer = 0.0f;
	private const float RACER_Z = -3.0f;
	
	void Start ()
	{
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
		Vector3 position = Vector3.zero;
		float half_w = spawnArea.size.x/2;
		float half_h = spawnArea.size.y/2;
		position.x = transform.position.x + Random.Range(-half_w, half_w);
		position.y = transform.position.y + Random.Range(-half_h, half_h);
		position.z = RACER_Z;
		GameObject clone = Instantiate(seed, position, Quaternion.identity);
		RacerMovement movementComponent = clone.GetComponent<RacerMovement>();
		movementComponent.linearMaxSpeed = Random.Range(
			linearMaxSpeed - linearMaxSpeedVar, linearMaxSpeed + linearMaxSpeedVar);
		movementComponent.enableSwing = (Random.Range(0.0f, 1.0f) > 0.5f);
		RacerPool.instance.Push(clone);
	}
}
