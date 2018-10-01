using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerGenerator : MonoBehaviour {

	public GameObject[] seeds;

	public BoxCollider2D spawnArea;
	public int gridWidth;
	public int gridHeight;
	public float spawnRatePercent;
	
	public float linearMaxSpeed;
	public float linearMaxSpeedVar;
	
	private const float RACER_Z = -3.0f;
	
	void Start ()
	{
		SpawnRacerRandom();
	}
	
	public void SpawnRacerRandom()
	{
		if(seeds.Length == 0)
		{
			return;
		}
		float half_w = spawnArea.size.x/2;
		float half_h = spawnArea.size.y/2;
		float x0 = transform.position.x - half_w;
		float max_x = transform.position.x + half_w;
		float y0 = transform.position.y - half_h;
		float max_y = transform.position.y + half_h;
		float x_step = spawnArea.size.x/gridWidth;
		float y_step = spawnArea.size.y/gridHeight;

		float x = x0;
		float y = y0;
		while(y <= max_y)
		{
			if(x > max_x)
			{
				x = x0;
				y += y_step;
			}
			float k = Random.Range(0.0f, 100.0f);
			if(k < spawnRatePercent)
			{
				int i = (int)Random.Range(0.0f, seeds.Length);
				if(i == seeds.Length)
				{
					i--;
				}
				Vector3 position = Vector3.zero;
				position.x = transform.position.x + x;
				position.y = transform.position.y + y;
				position.z = RACER_Z;
				GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
				RacerMovement movement_component = clone.GetComponent<RacerMovement>();
				movement_component.linearMaxSpeed = Random.Range(
					linearMaxSpeed - linearMaxSpeedVar, linearMaxSpeed + linearMaxSpeedVar);
				movement_component.enableSwing = (Random.Range(0.0f, 1.0f) > 0.5f);
				// TODO: implement a pool that actually is a pool
				RacerPool.instance.Push(clone);
			}
			x += x_step;
		}
		
	}
}
