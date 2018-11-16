using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public enum State
	{
		Running,
		Pausing,
		ToRunning,
		CrashDrama,
		GameOver
	}

	public enum DifficultyFunction
	{
		Linear,
		Sin,
		Exponential
	}

	public static GameManager instance = null;
    public int characterToSpawn;
	public GameObject mainCharacter;
	public GameObject gameOverPopup;
	public float difficulty;
	public float difficultyFactorA;
	public float difficultyFactorB;
	public DifficultyFunction difficultyGrowth;
	public State state;

	public float pauseCountDownTime;
	public float pauseCountDownTimer;
	

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

	void Start () 
	{
		pauseCountDownTimer = 0.0f;
		state = State.Running;
		difficulty = 0.0f;
	}
	
	void Update()
	{
		switch(state)
		{
			case State.Running:
				if(Input.GetButtonDown("Pause"))
				{
					Pause();
				}
			break;
			case State.Pausing:
				if(Input.GetButtonDown("Pause"))
				{
					UnpauseBegin();
				}
			break;
			case State.ToRunning:
				pauseCountDownTimer -= Time.unscaledDeltaTime;
				if(pauseCountDownTimer <= 1.0f)
				{
					UnpausedEnd();
				}
			break;
		}
	}

	void FixedUpdate()
	{
        if(mainCharacter == null)
        {
            return;
        }
		float y = mainCharacter.transform.position.y;
		switch(difficultyGrowth)
		{
			case DifficultyFunction.Linear:
				difficulty = y*difficultyFactorA;
			break;
			case DifficultyFunction.Sin:
				difficulty = Mathf.Sin(y*difficultyFactorA)*difficultyFactorB;
			break;
			case DifficultyFunction.Exponential:
				difficulty = Mathf.Pow(y*difficultyFactorA, difficultyFactorB);
			break;
			default:
				difficulty = 0.0f;
				break;
		}
		difficulty = Mathf.Clamp(difficulty, 0.7f, 2.0f);
		//Debug.Log("y"+y+" difficulty = "+difficulty);
	}

	public void Pause()
	{
		state = State.Pausing;
		Time.timeScale = 0.0f;
	}

	public void UnpauseBegin()
	{
		state = State.ToRunning;
		pauseCountDownTimer = pauseCountDownTime;
	}

	public void UnpausedEnd()
	{
		state = State.Running;
		Time.timeScale = 1.0f;
	}
	
	public void GameRestart()
	{
		state = State.ToRunning;
		pauseCountDownTimer = pauseCountDownTime;
		StartCoroutine(ReloadScene());
	}

	public void ToMainMenu()
	{
		StartCoroutine(LoadMainMenuScene());
	}

	private IEnumerator LoadMainMenuScene()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
		while (!operation.isDone)
        {
            yield return null;
        }
		Time.timeScale = 1.0f;
	}

	private IEnumerator ReloadScene()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		while (!operation.isDone)
        {
            yield return null;
        }
	}
	
	public void GameOver(int reason)
	{
		Time.timeScale = 0.0f;
		GameObject clone = Instantiate(gameOverPopup);
		clone.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
		clone.GetComponent<GameOverPopup>().SetCrashReason(reason);
		state = State.GameOver;
	}
	static public void DrawGizmoString(Vector3 worldPos, string text, Color? colour = null) 
	{
		UnityEditor.Handles.BeginGUI();

		var restoreColor = GUI.color;

		if (colour.HasValue) GUI.color = colour.Value;
		var view = UnityEditor.SceneView.currentDrawingSceneView;
		Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

		if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
		{
			GUI.color = restoreColor;
			UnityEditor.Handles.EndGUI();
			return;
		}

		Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
		GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
		GUI.color = restoreColor;
		UnityEditor.Handles.EndGUI();
	}
}
