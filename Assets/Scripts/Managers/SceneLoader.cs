using System.Collections;
using UnityEngine; 
using UnityEngine.SceneManagement;


public class SceneLoader : Singleton<SceneLoader>
{
	// Use this for initialization
	protected override void Awake()
	{
		base.Awake();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			OnEscape();
	}

	// HACK FRS 161027 Lo suyo sería ir apilando escenas y sacar la anterior....
	private void OnEscape()
	{
		switch(SceneManager.GetActiveScene().name)
		{
			case "MainMenu":
				Debug.Log("<b>APPLICATION EXIT</b>");
				Application.Quit();
				break;

			default:
				GoToMainMenu();
				break;
		}
	}

	public void GoToMainMenu()
	{
		LoadSceneAsync("MainMenu");
	}

	public void GoToParentsMenu()
	{
		LoadSceneAsync("ParentsWebView");
	}

	public void GoToQRUnlock(string previousScene)
	{
		PreviousScene = previousScene;
		LoadSceneAsync("QRMenu");
	}

	public void GoToSelectionMenu()
	{
		if (!_alreadyShown && !PlayerPrefs.HasKey("Room 1"))
		{
			_alreadyShown = true;
			GoToQRUnlock("SelectionMenu");
		}
		else
			LoadSceneAsync("SelectionMenu");
			//SceneManager.LoadScene("SelectionMenu");
	}

	public void GoBackToPreviousScene()
	{
		if (PreviousScene != string.Empty)
		{
			LoadSceneAsync(PreviousScene);
			//SceneManager.LoadScene(PreviousScene);
		}
		else
		{
			Debug.Log("<color=orange> Previous Scene not declared! </color>. Set previousScene name before calling 'GoBackToPreviousScene()'");
			LoadSceneAsync("SelectionMenu");
			//SceneManager.LoadScene("SelectionMenu");
		}
	}

	public void GoToRoom(string roomName)
	{
		Instance.StartCoroutine(GoToRoomCoroutine(roomName));
	}

	

	#region Private Methods

	private void LoadSceneAsync(string sceneName)
	{
		Instance.StartCoroutine(LoadSceneCoroutine(sceneName));
	}
		   
	private IEnumerator GoToRoomCoroutine(string roomName)
	{
		Transition.Instance.StartAnim(1f);
		yield return new WaitForSeconds(.9f);
		yield return LoadSceneCoroutine(roomName);
	}

	private IEnumerator LoadSceneCoroutine(string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
		if(operation == null || operation.isDone)
			yield break;

		yield return new WaitForSeconds(.5f);
		if (operation.isDone)
			yield break;

		LoadScreen.Instance.Show(sceneName);
		yield return new WaitUntil(() => {
			LoadScreen.Instance.ProgressRatio = operation.progress;
			return operation.isDone;
		});

		LoadScreen.Instance.IsVisible = false;
	}

	#endregion


	//===========================================================

	#region Events

	
	

	#endregion

	//============================================================

	public int BoomonLinked = -1;
	private static bool _alreadyShown = false;
	private string PreviousScene;
}
