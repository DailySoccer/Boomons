using UnityEngine;

public class PlayerFollower : MonoBehaviour
{

	private void Awake()
	{
		_forward = transform.forward;
		_right = transform.right;
		_up = transform.up;
	}


	// Use this for initialization
	private void Start ()
	{
		if (_boomon == null)
			_boomon = FindObjectOfType<BoomonController>();

		_lastBoomonPos = _boomon.Position;
	}

	private void OnDestroy()
	{
		_boomon = null;
	}
	
	private void LateUpdate ()
	{
		Vector3 boomonPos = _boomon.Position;

		Vector3 pos = boomonPos - _distance.z * _forward + _distance.y * _up;

		if (_boomon.CurrentState != BoomonController.State.Throwing)
		{
			if (_boomon.CurrentState == BoomonController.State.Moving)
				pos += _distance.x*_boomon.transform.forward;
			else
				pos += .2f* _distance.z*_forward;

			transform.position = Vector3.Lerp(transform.position, pos, _speed * Time.deltaTime);
		}
		else
		{
			transform.position = pos;
		}


		//transform.LookAt(boomonPos);
		

		//_lastBoomonPos = boomonPos;
	}



	[SerializeField] private BoomonController _boomon;
	[SerializeField, Range(1f, 20f)] private float _speed;
	[SerializeField, Range(1f, 20f)] private float _rotationSpeed;
	[SerializeField] private Vector3 _distance;

	private Vector3 _lastBoomonPos;
	private Vector3 _forward;
	private Vector3 _right;
	private Vector3 _up;
	
}
