using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject trackPool;
	public GameObject racerPool;
    public GameObject playerManager;

	void Awake ()
	{
        if (GameManager.instance == null && gameManager != null)
		{
			Instantiate(gameManager);
		}
		else
		{
			gameManager = GameManager.instance.gameObject;
		}
        if (TrackPool.instance == null && trackPool != null)
		{
			Instantiate(trackPool);
		}
		else
		{
			trackPool = TrackPool.instance.gameObject;
		}
        if (RacerPool.instance == null && racerPool != null)
		{
			Instantiate(racerPool);
		}
		else
		{
			racerPool = RacerPool.instance.gameObject;
		}
        if (PlayerManager.instance == null && playerManager != null)
        {
            Instantiate(playerManager);
        }
		else
		{
			playerManager = PlayerManager.instance.gameObject;
		}
	}
}
