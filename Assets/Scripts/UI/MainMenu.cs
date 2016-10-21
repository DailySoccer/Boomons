using UnityEngine;

public class MainMenu : MonoBehaviour
{
	
	public void OnSceneClick(string sceneName)
	{
		MetaManager.GetManager<GameManager>().LoadScene(sceneName);
	}
}
