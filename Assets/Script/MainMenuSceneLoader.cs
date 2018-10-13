using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneLoader : MonoBehaviour {

	void Start ()
	{
		StartCoroutine(LoadScene());
	}
	
	IEnumerator LoadScene()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
		while (!operation.isDone)
        {
            yield return null;
        }
	}
}
