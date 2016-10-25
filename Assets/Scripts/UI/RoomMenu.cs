using UnityEngine;

public class RoomMenu : MonoBehaviour
{
	
	public void OnSceneClick(string sceneName)
	{
		MetaManager.Instance.GetManager<GameManager>().LoadScene(sceneName);
	}
}
