using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class SceneLoader : Singleton<SceneLoader>
{

	// Use this for initialization
	void Start () {
	
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			OnEscape();
	}

	// HACK FRS 161027 Lo suyo sería ir apilando escenas y sacar la anterior....
	private void OnEscape()
	{
		switch(SceneManager.GetActiveScene().name)
		{
			case "MainMenu":
				Application.Quit();
				break;

			default:
				GoToMainMenu();
				break;
		}
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
		SceneManager.LoadScene("SelectionMenu");
	}

	public void GoToUnblockRoom(string roomId)
	{
		Debug.Log("GoToUnblock>> " + roomId);
	}
}
