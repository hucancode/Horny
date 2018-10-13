using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(LoadScene());
	}
	
	IEnumerator LoadScene()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync("GamePlay");
		while (!operation.isDone)
        {
            yield return null;
        }
	}
}
