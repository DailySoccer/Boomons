using UnityEngine;

public class MainMenu : MonoBehaviour
{
	
	public void OnSceneClick(string sceneName)
	{
		GameManager.Instance.LoadScene(sceneName);
	}
}
