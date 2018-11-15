using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPasserGenerator : MonoBehaviour {

	public GameObject[] seeds;
	public Vector2 spawnArea;
	public float interval;
	public float linearMaxSpeed;
	public float linearMaxSpeedVar;
	public bool lookAtLeft;

	private float timer;
	private const float RACER_Z = -3.0f;
	
	void Start ()
	{
		timer = 0.0f;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 0, 0, 0.2f);
		Gizmos.DrawCube(transform.position, new Vector3(spawnArea.x, spawnArea.y, 1));
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
		float half_w = spawnArea.x/2;
		float half_h = spawnArea.y/2;
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
