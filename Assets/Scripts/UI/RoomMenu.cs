using UnityEngine;

public class RoomMenu : MonoBehaviour
{
	
	public void OnSceneClick(string sceneName)
	{
		MetaManager.GetManager<GameManager>().LoadScene(sceneName);
	}
}
