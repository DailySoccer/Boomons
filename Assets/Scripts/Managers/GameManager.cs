

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	#region Public Methods

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	#endregion

	//=======================================================

	#region Mono

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			OnEscape();
	}

	#endregion

	//====================================================

	#region Events

	private void OnEscape()
	{
		if(SceneManager.GetActiveScene().name == _mainMenuScene)
			Application.Quit();
		else
			LoadScene(_mainMenuScene);
	}

	#endregion


	//========================================================

	#region Private Fields

	[SerializeField] private string _mainMenuScene = "MainMenu";

	#endregion

}
