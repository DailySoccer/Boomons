using UnityEngine;

public class RoomMenu : MonoBehaviour
{
	#region Public Fields

	public string SelectedRoom { get; private set; }

	#endregion

	//=========================================================================

	#region Events

	public void OnRoomClick(string roomName)
	{
		SelectedRoom = roomName;
		Transition.Instance.AnimEnd += OnTransitionEnd;
		Transition.Instance.StartAnim(_transitionSecs);
	}


	private void OnTransitionEnd()
	{
		MetaManager.Instance.GetManager<GameManager>().LoadScene(SelectedRoom);
	}

	#endregion

	//===============================================================

	#region Private Fields

	[SerializeField, Range(0f, 10f)] private float _transitionSecs = 2f;

	#endregion
}

