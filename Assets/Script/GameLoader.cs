using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject trackPool;
	public GameObject racerPool;
	public GameObject crashManager;
    public GameObject playerManager;

	void Awake ()
	{
        if (GameManager.instance == null && gameManager != null)
		{
			Instantiate(gameManager);
		}
        if (TrackPool.instance == null && trackPool != null)
		{
			Instantiate(trackPool);
		}
        if (RacerPool.instance == null && racerPool != null)
		{
			Instantiate(racerPool);
		}
        if (CrashManager.instance == null && crashManager != null)
		{
			Instantiate(crashManager);
		}
        if (PlayerManager.instance == null && playerManager != null)
        {
            Instantiate(playerManager);
        }
	}
}
