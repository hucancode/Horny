using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance = null;

    public GameObject[] racers;
	public Sprite[] avatars;
	
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		if(instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	void Start()
	{
	}
	
	public void Spawn()
    {
        if(racers.Length == 0)
        {
            return;
        }
        int i = GameManager.instance.characterToSpawn;
        i = Mathf.Clamp(i, 0, racers.Length - 1);
		Instantiate(racers[i], Vector3.zero, Quaternion.identity);
	}
}
