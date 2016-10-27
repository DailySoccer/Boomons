using UnityEngine;

public class RoomMenu : MonoBehaviour
{
	#region Public Fields

	public string SelectedRoom { get; private set; }

	#endregion

	//=========================================================================

	#region Mono

	


	#endregion

	//=================================================================

	#region Events

	public void OnRoomClick(RoomButton button)
	{
		if (!button.IsBlocked)
		{
			SelectedRoom=button.TargetRoomId;
			Transition.Instance.AnimEnd += OnTransitionEnd;
			Transition.Instance.StartAnim(_transitionSecs);
		}
		else
		{
			SceneLoader.Instance.GoToParentsMenu();
		}
	}


	private void OnTransitionEnd()
	{
		Transition.Instance.AnimEnd -= OnTransitionEnd;
		MetaManager.Instance.GetManager<GameManager>().StartRoom(SelectedRoom);
	}

	#endregion

	//===============================================================

	#region Private Fields

	[SerializeField, Range(0f, 10f)] private float _transitionSecs = 2f;

	#endregion
}

