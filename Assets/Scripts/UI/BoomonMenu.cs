using UnityEngine;

public class BoomonMenu : MonoBehaviour
{
	#region Public Methods

	public void OnBoomonSelected(BoomonController boomon)
	{
		MetaManager.Instance.GetManager<GameManager>().BoomonRole = boomon.Role;

		_cameraFollower.Target = boomon.transform;
		_cameraFollower.StopZoomRatio = _selectionZoomRatio;
		//boomon.GoTo(_roomMenu.transform.position);
	}

	#endregion

	//======================================================================

	#region Mono

	private void Awake()
	{
		_camera = Camera.main;
		_cameraFollower = _camera.GetComponent<ScrollFollower>();

		Debug.Assert(_cameraFollower != null, "BoomonMenu::Awake>> CameraFollower not found!", _camera);
	}

	private void OnDestroy()
	{
		_roomMenu = null;
		_camera = null;
		_cameraFollower = null;
	}

	#endregion

	[SerializeField] private Transform _roomMenu; 
	private Camera _camera;
	private ScrollFollower _cameraFollower;

	[SerializeField, Range(0f,1f)] private float _selectionZoomRatio = .2f;
}
