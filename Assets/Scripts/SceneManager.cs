

using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{ 
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

}
