using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPasserGenerator : MonoBehaviour {

	public GameObject[] seeds;
	public BoxCollider2D spawnArea;
	public float interval;
	public float linearMaxSpeed;
	public float linearMaxSpeedVar;
	public bool lookAtLeft;

	private float timer;
	private const float RACER_Z = -3.0f;
	
	void Start ()
	{
		interval = 50.0f;
		linearMaxSpeed = 120.0f;
		linearMaxSpeedVar = 50.0f;
		lookAtLeft = false;
		timer = 0.0f;
	}
	
	void FixedUpdate ()
	{
		timer -= Time.fixedDeltaTime;
		if(timer <= 0.0f)
		{
			timer = interval;
			SpawnPasserRandom();
		}
	}

	public void SpawnPasserRandom()
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
		CrossPasserMovement movement_component = clone.GetComponent<CrossPasserMovement>();
		SpriteRenderer sprite_component = clone.GetComponent<SpriteRenderer>();
		movement_component.linearMaxSpeed = Random.Range(
			linearMaxSpeed - linearMaxSpeedVar, linearMaxSpeed + linearMaxSpeedVar);
		if(lookAtLeft)
		{
			sprite_component.flipX = true;
			movement_component.linearMaxSpeed = - movement_component.linearMaxSpeed;
		}
		RacerPool.instance.Push(clone);
	}
}
