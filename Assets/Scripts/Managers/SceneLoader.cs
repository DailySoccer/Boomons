using UnityEngine; 
using UnityEngine.SceneManagement;


public class SceneLoader : Singleton<SceneLoader>
{
	// Use this for initialization
	void Start () {
	
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
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToParentsMenu()
	{
		Application.OpenURL("http://www.unusualwonder.com/");
		PlayerPrefs.DeleteKey("Room 1");
		PlayerPrefs.DeleteKey("Room 2");
		PlayerPrefs.DeleteKey("Room 3");
		PlayerPrefs.DeleteKey("Room 4");
		PlayerPrefs.Save();
	}

	public void GoToQRUnlock(string previousScene)
	{
		PreviousScene = previousScene;
		SceneManager.LoadScene("QRMenu");
	}

	public void GoToSelectionMenu()
	{
		if (PlayerPrefs.GetString("Room 1") == string.Empty)
		{
			GoToQRUnlock("SelectionMenu");
		}
		else
		{
			SceneManager.LoadScene("SelectionMenu");
		}
	}

	public void GoBackToPreviousScene()
	{
		if (PreviousScene != string.Empty)
		{
			SceneManager.LoadScene(PreviousScene);
		}
		else
		{
			Debug.Log("<color=orange> Previous Scene not declared! </color>. Set previousScene name before calling 'GoBackToPreviousScene()'");
			SceneManager.LoadScene("SelectionMenu");
		}
	}

	public int BoomonLinked = -1;
	private string PreviousScene;
}
