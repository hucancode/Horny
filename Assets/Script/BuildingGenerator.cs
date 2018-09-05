using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour {

	public enum MarchingDirection
	{
		Horizontal,
		Vertical
	}
	public GameObject[] seeds;
	public BoxCollider2D spawnArea;
	public MarchingDirection direction;
	public float margin;
	public float marginVar;
	public bool flip;

	private const float HOUSE_Z = 0.0f;
	private const float HOUSE_Z_INC = 1.0f;
	
	void Start ()
	{
		if(direction == MarchingDirection.Vertical)
		{
			float y = 0;
			float max_y = y + spawnArea.size.y * transform.lossyScale.y;
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
		else if(direction == MarchingDirection.Horizontal)
		{
			float x = 0;
			float max_x = x + spawnArea.size.x * transform.lossyScale.x;
			float z = 0.0f;
			while(x < max_x)
			{
				int i = (int)Random.Range(0.0f, seeds.Length);
				if(i == seeds.Length)
				{
					i--;
				}

				Vector3 position = Vector3.zero;
				position.x = transform.position.x + x;
				position.y = transform.position.y;
				position.z = HOUSE_Z + z;
				z += HOUSE_Z_INC;

				GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
				BoxCollider2D collider = clone.GetComponent<BoxCollider2D>();
				SpriteRenderer sprite_renderer = clone.GetComponent<SpriteRenderer>();

				x += collider.size.x * clone.transform.lossyScale.x;
				if(x >= max_x)
				{
					Destroy(clone);
					break;
				}
				x += Random.Range(margin - marginVar, margin + marginVar);

				clone.transform.parent = transform;
				sprite_renderer.flipX = flip;
			}
		}
	}
}
