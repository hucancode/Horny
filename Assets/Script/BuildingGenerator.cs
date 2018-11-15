using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour {
	public GameObject[] seeds;
	public Vector2 spawnAreaVector;
	public float margin;
	public float marginVar;
	public bool flip;

	private const float HOUSE_Z = 0.0f;
	private const float HOUSE_Z_INC = 1.0f;
	
	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 1, 0, 0.2f);
		Gizmos.DrawCube(transform.position, new Vector3(spawnAreaVector.x, spawnAreaVector.y, 1));
	}

	void Start ()
	{
		float y = -spawnAreaVector.y*0.5f;
		float max_y = y + spawnAreaVector.y;
		float z = 0.0f;
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
			position.z = HOUSE_Z + z;
			z += HOUSE_Z_INC;

			GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
			BoxCollider2D collider = clone.GetComponent<BoxCollider2D>();
			SpriteRenderer sprite_renderer = clone.GetComponent<SpriteRenderer>();

			y += collider.size.y * clone.transform.lossyScale.y;
			if(y >= max_y)
			{
				Destroy(clone);
				break;
			}
			y += Random.Range(margin - marginVar, margin + marginVar);

			clone.transform.parent = transform;
			sprite_renderer.flipX = flip;
		}
	}
}
