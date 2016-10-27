using UnityEngine;

public class RoomButton : MonoBehaviour
{

	#region Public Fields

	public bool IsBlocked 
	{
		get { return _blocker != null && _blocker.activeSelf;  }
		set {
			if(_blocker !=null) 
				_blocker.SetActive(value);
		}
	}

	public string TargetRoomId { get { return _targetRoomId; } }

	#endregion

	//=================================================================

	#region Mono

	private void Awake()
	{
		if(_blocker==null) {
		    Transform blockerTransform = transform.FindChild(_blockerName);
			if(blockerTransform!=null)
				_blocker=blockerTransform.gameObject;
		}
	}

	private void OnDestroy()
	{
		_blocker=null;
	}


	private void Start()
	{
		IsBlocked= !PlayerPrefs.HasKey(_targetRoomId);
	}

	#endregion

	//=================================================================

	#region Private Fields
	[SerializeField] private string _blockerName = "roomBlocker";
	[SerializeField] private GameObject _blocker;
	[SerializeField] private string _targetRoomId = "Room 1";
	#endregion


}
