using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour {

	public GameObject[] seeds;
	public BoxCollider2D hitBox;
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		//Debug.Log("OnTriggerEnter2D");
		if(coll.gameObject.tag != "Racer")
		{
			return;
		}
		
		GameObject track_object = transform.parent.gameObject;
		SpriteRenderer track_sprite = track_object.GetComponent<SpriteRenderer>();
		float track_length = track_sprite.size.y * track_object.transform.lossyScale.y;
		Vector3 position = Vector3.zero;
		position.x = transform.parent.position.x;
		position.y = transform.parent.position.y + track_length;
		position.z = transform.parent.position.z;

		int i = (int)Random.Range(0.0f, seeds.Length);
		if(i == seeds.Length)
		{
			i--;
		}
		GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
		// TODO: implement track pool that actually is a pool
		TrackPool.instance.Push(clone);
		hitBox.enabled = false;
	}
}
