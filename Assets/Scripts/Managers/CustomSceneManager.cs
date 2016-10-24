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
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToParentsMenu()
	{
		SceneManager.LoadScene("ParentsMenu");
	}

	public void GoToPlay()
	{
		SceneManager.LoadScene("RoomMenu");
	}
}
