using UnityEngine;

public class PlayerFollower : MonoBehaviour
{

	private void Awake()
	{
		_forward = transform.forward;
		_right = transform.right;
		_up = transform.up;

		_wallLayerMask = LayerMask.GetMask(_wallLayerName);
	}

	private void OnDestroy()
	{
		_boomon = null;
	
	}

	// Use this for initialization
	private void Start()
	{
		if (_boomon == null)
			_boomon = FindObjectOfType<BoomonController>();

		_lastBoomonPos = _boomon.Position;
	}


	private void LateUpdate()
	{
		Vector3 boomonPos = _boomon.Position;

		Vector3 pos = boomonPos - _distance.z*_forward + _distance.y*_up;

		if (_boomon.CurrentState != BoomonController.State.Throwing)
		{
			if (_boomon.CurrentState == BoomonController.State.Moving)
				pos += _distance.x*_boomon.transform.forward;
			else
				pos += .2f*_distance.z*_forward;

			if (_isCrashingWall)
				pos -= Vector3.Project(pos - transform.position, transform.right);

			transform.position = Vector3.Lerp(transform.position, pos, _speed*Time.deltaTime);
		}
		else
		{
			transform.position = pos;
		}


		// TODO establecer referencias del scroll 
		// transform.LookAt(boomonPos);


		//_lastBoomonPos = boomonPos;
	}


	private void FixedUpdate()
	{
		RaycastHit hitInfo;
		_isCrashingWall = (Physics.Raycast(transform.position, transform.right, out hitInfo, _wallDistanceMax, _wallLayerMask)
		                   || Physics.Raycast(transform.position, -transform.right, out hitInfo, _wallDistanceMax, _wallLayerMask))
		                  && Vector3.Distance(transform.position, hitInfo.point) < _wallDistanceMin*_wallDistanceMin;
		
		
		if(_isCrashingWall)
			transform.position = hitInfo.point + hitInfo.normal*_wallDistanceMin;
	}






	
	[SerializeField] private BoomonController _boomon;
	[SerializeField, Range(1f, 20f)] private float _speed;
	[SerializeField, Range(1f, 20f)] private float _rotationSpeed;
	[SerializeField] private Vector3 _distance;
	[SerializeField, Range(0f, 10f)] private float _wallDistanceMin;
	[SerializeField, Range(0f, 100f)] private float _wallDistanceMax;
	[SerializeField] private string _wallLayerName = "Ground";

	private Vector3 _lastBoomonPos;
	private Vector3 _forward;
	private Vector3 _right;
	private Vector3 _up;
	private Vector3 _lastPos;
	private bool _isCrashingWall;
	private int _wallLayerMask;
	
}
