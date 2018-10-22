using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMainCharacter : MonoBehaviour {

	private Vector3 offset;
	private bool following;
	float fixedX;
	float fixedZ;
	void Start () 
	{
		following = false;
	}
	
	void Update () 
	{
		if(following)
		{
			if(GameManager.instance.mainCharacter == null)
			{
				following = false;
				return;
			}
			Vector3 cam_position = GameManager.instance.mainCharacter.transform.position + offset;
			cam_position.x = fixedX;
			cam_position.z = fixedZ;
			transform.position = cam_position;
		}
		else
		{
			if(GameManager.instance.mainCharacter != null)
			{
				offset = transform.position - GameManager.instance.mainCharacter.transform.position;
				following = true;
				fixedX = transform.position.x;
				fixedZ = transform.position.z;
				return;
			}
		}
	}
}
