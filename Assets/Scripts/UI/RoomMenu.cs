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
		var game = MetaManager.Instance.Get<GameManager>();
		game.Room = button.TargetRoomId;

		if (button.IsBlocked)
			SceneLoader.Instance.GoToQRUnlock("SelectionMenu");
		else if (game.IsReadyToPlay)
			game.Play();
		else
			Camera.main.GetComponent<Animator>().SetTrigger("ShowBoomons");		  
	}




	#endregion

	//===============================================================

	#region Private Fields



	#endregion
}

