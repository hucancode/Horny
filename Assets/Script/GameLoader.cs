using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject trackPool;
	public GameObject racerPool;
	
	void Awake ()
	{
		if (GameManager.instance == null)
		{
			Instantiate(gameManager);
		}
		if (TrackPool.instance == null)
		{
			Instantiate(trackPool);
		}
		if (RacerPool.instance == null)
		{
			Instantiate(racerPool);
		}
	}
}
