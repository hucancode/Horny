using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour {
	public GameObject[] seeds;
	public Vector2 spawnArea;
	public float margin;
	public float marginVar;
	public bool flip;
	
	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 1, 0, 0.2f);
		Gizmos.DrawCube(transform.position, new Vector3(spawnArea.x, spawnArea.y, 1));
		//GameManager.DrawGizmoString(transform.position, "Building Generator");
	}

	void Start ()
	{
		float y = -spawnArea.y*0.5f;
		float max_y = y + spawnArea.y;
		while(y < max_y)
		{
			int i = (int)Random.Range(0.0f, seeds.Length);
			if(i == seeds.Length)
			{
				i--;
			}

			Vector3 position = Vector3.zero;
			position.x = transform.position.x;
			position.y = transform.position.y + y;
			position.z = position.y % 100.0f;
			
			bool buggy = position.z > 90.0f;
			// this method of moduling y for z has some downside, we need to hide it
			// by avoiding buggy case
			if(buggy)
			{
				y += 100 - position.z;
				continue;
			}

			GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
			BoxCollider2D collider = clone.GetComponent<BoxCollider2D>();
			SpriteRenderer sprite_renderer = clone.GetComponent<SpriteRenderer>();

			y += collider.size.y;
			y += Random.Range(margin - marginVar, margin + marginVar);
			if(y >= max_y)
			{
				Destroy(clone);
				break;
			}
			clone.transform.parent = transform;
			sprite_renderer.flipX = flip;
		}
	}
}
