using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour {

	public GameObject[] seeds;
	public BoxCollider2D hitBox;
	public float trackLength;
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		//Debug.Log("OnTriggerEnter2D");
		if(coll.gameObject != GameManager.instance.mainCharacter)
		{
			return;
		}
		
		Vector3 position = Vector3.zero;
		position.x = transform.parent.position.x;
		position.y = transform.parent.position.y;
		position.z = transform.parent.position.z;

		int i = (int)Random.Range(0.0f, seeds.Length);
		if(i == seeds.Length)
		{
			i--;
		}
		TrackGenerator next_track_info = seeds[i].GetComponentInChildren<TrackGenerator>();
		float next_track_length = next_track_info.trackLength;
		position.y +=  trackLength/2.0f + next_track_length/2.0f;
		GameObject clone = Instantiate(seeds[i], position, Quaternion.identity);
		// TODO: implement track pool that actually is a pool
		TrackPool.instance.Push(clone);
		hitBox.enabled = false;
	}
}
