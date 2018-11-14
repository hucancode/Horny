using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerGenerator : MonoBehaviour {

	public GameObject[] seeds;

	public BoxCollider2D spawnArea;
	public BoxCollider2D wave2Trigger;
	public Vector2 spawnAreaVector;
	public int gridWidth;
	public int gridHeight;
	public float spawnRatePercent;
	
	private const float RACER_Z = -3.0f;
	
	void Start()
	{
		Debug.Log("i am "+gameObject.transform.parent.gameObject+", spawn wave 1");
		StartCoroutine(SpawnRacerRandom());
		//SpawnRacerRandom();
	}

	void OnTriggerEnter2D(Collider2D coll)
    {
		//Debug.Log("OnTriggerEnter2D");
		if(coll.gameObject != GameManager.instance.mainCharacter)
		{
			return;
		}
		wave2Trigger.enabled = false;
		Debug.Log("i am "+gameObject.transform.parent.gameObject+", spawn wave 2");
		StartCoroutine(SpawnRacerRandom());
		//SpawnRacerRandom();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 0, 0, 0.2f);
		Gizmos.DrawCube(transform.position, new Vector3(spawnAreaVector.x, spawnAreaVector.y, 1));
	}
	
	public IEnumerator SpawnRacerRandom()
	{
		if (seeds.Length == 0)
		{
			yield return null;
		}
		while (GameManager.instance.mainCharacter == null)
        {
            yield return null;
        }
		float half_w = spawnArea.size.x/2.0f;
		float half_h = spawnArea.size.y/2.0f;
		float x0 = transform.position.x - half_w;
		float max_x = transform.position.x + half_w;
		float y0 = transform.position.y - half_h;
		float max_y = transform.position.y + half_h;
		float x_step = spawnArea.size.x/gridWidth;
		float y_step = spawnArea.size.y/gridHeight;

		float x = x0;
		float y = y0;
		//Debug.Log("spawn racer setup y0="+y0+", max_y="+max_y+", y_step="+y_step);
		float rate = spawnRatePercent*GameManager.instance.difficulty;
		while(y <= max_y)
		{
			if(x > max_x)
			{
				x = x0;
				y += y_step;
			}
			float k = Random.Range(0.0f, 100.0f);
			if(k < rate)
			{
				int i = (int)Random.Range(0.0f, seeds.Length);
				if(i == seeds.Length)
				{
					i--;
				}
				Vector3 position = Vector3.zero;
				position.x = x;
				position.y = y;
				position.z = RACER_Z;
				//Debug.Log("spawn racer at "+position);
				GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
				clone.transform.parent = transform;
				RacerMovement movement_component = clone.GetComponent<RacerMovement>();
				RacerMovement movement_component_player = GameManager.instance.mainCharacter.GetComponent<RacerMovement>();

				float seed_speed;
				if(transform.position.y < GameManager.instance.mainCharacter.transform.position.y)
				{
					seed_speed = movement_component_player.linearSpeed*0.75f;
				}
				else
				{
					seed_speed = movement_component_player.linearSpeed*1.5f;
				}

				movement_component.linearSpeed = seed_speed;
				movement_component.linearSpeed = Mathf.Clamp(seed_speed, movement_component.linearMaxSpeed, movement_component.linearMinSpeed);
				movement_component.linearMaxSpeed = movement_component.linearSpeed;
				// TODO: implement a pool that actually is a pool
				RacerPool.instance.Push(clone);
			}
			x += x_step;
		}
		Debug.Log("spawn finished");
	}
}
