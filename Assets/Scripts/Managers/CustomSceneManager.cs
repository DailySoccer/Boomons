using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CustomSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene("MainMenuScene");
	}

	public void GoToParentsMenu()
	{
		SceneManager.LoadScene("ParentsMenuScene");
	}

	public void GoToPlay()
	{
		SceneManager.LoadScene("");
	}
}
