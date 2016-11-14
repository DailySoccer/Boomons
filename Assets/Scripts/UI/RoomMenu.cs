using UnityEngine;

public class RoomMenu : MonoBehaviour
{
	#region Public Fields


	#endregion

	//=========================================================================

	#region Mono




	#endregion

	//=================================================================

	#region Events

	public void OnRoomClick(RoomButton button)
	{
		if (!button.IsBlocked)
			MetaManager.Instance.GetManager<GameManager>().StartRoom(button.TargetRoomId);
		else
			SceneLoader.Instance.GoToQRUnlock("SelectionMenu");
	}




	#endregion

	//===============================================================

	#region Private Fields



	#endregion
}

