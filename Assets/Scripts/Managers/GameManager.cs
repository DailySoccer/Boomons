

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

	private void OnEnable()
	{
		TouchManager.Instance.Back += OnBack;
	}

	private void OnDisable()
	{
		TouchManager.Instance.Back -= OnBack;
	}

	#endregion

	//====================================================

	#region Events

	private void OnBack()
	{
		LoadScene(_mainMenuScene);
	}

	#endregion


	//========================================================

	#region Private Fields

	[SerializeField] private string _mainMenuScene = "MainMenu";

	#endregion

}
