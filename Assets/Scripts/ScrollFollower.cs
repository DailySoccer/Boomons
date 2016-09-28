using System;

using UnityEngine;

public class ScrollFollower : MonoBehaviour
{

	#region Public Fields
	#endregion


	//=================================================

	#region Mono

	private void Awake()
	{
		_forward = transform.forward;
		_right = transform.right;
		_up = transform.up;
	}

	private void OnDestroy()
	{
		_target = null;

	}

	private void Start()
	{
		_target = GameObject.FindGameObjectWithTag(_targetTag).transform;
		_lastPos = _target.position;
	}


	private void LateUpdate()
	{
		if(!_target.gameObject.activeInHierarchy)
			_target = GameObject.FindGameObjectWithTag(_targetTag).transform;
		
		Vector3 targetPos = _target.position;
		Vector3 deltaPos = targetPos - _lastPos;

		Vector3 cameraPos = targetPos;
		cameraPos += _distance.y * _up;
		cameraPos -= _stoppedDistanceRatio*_distance.z*_forward;

		int direction = Math.Sign(Vector3.Dot(deltaPos, _right));

		cameraPos -= Math.Abs( direction ) * (1f - _stoppedDistanceRatio) *_distance.z * _forward;
		cameraPos += direction * _distance.x * _right;

		Vector3 fwdDeltaPos = Vector3.Project(cameraPos - transform.position, _forward);
		transform.position = Vector3.Lerp(transform.position, cameraPos - fwdDeltaPos, _lateralSpeed * Time.deltaTime);;
		transform.position = Vector3.Lerp(transform.position, cameraPos, _depthSpeed * Time.deltaTime);

		transform.forward = (_target.position - transform.position).normalized;

		_lastPos = targetPos;
	}


	//private void FixedUpdate()
	//{
	//	RaycastHit hitInfo;
	//	_isCrashingWall = (Physics.Raycast(transform.position, transform.right, out hitInfo, _wallDistanceMax, _wallLayerMask)
	//					   || Physics.Raycast(transform.position, -transform.right, out hitInfo, _wallDistanceMax, _wallLayerMask))
	//					  && Vector3.Distance(transform.position, hitInfo.point) < _wallDistanceMin * _wallDistanceMin;


	//	if (_isCrashingWall)
	//		transform.position = hitInfo.point + hitInfo.normal * _wallDistanceMin;
	//}



	#endregion


		//=================================================================



	[SerializeField] private Transform _target;
	[SerializeField] private string _targetTag = "Player";
	[SerializeField] private Vector3 _distance;
	[SerializeField, Range(0f, 1f)] private float _stoppedDistanceRatio;
	[SerializeField, Range(1f, 20f)] private float _depthSpeed;
	[SerializeField, Range(1f, 20f)] private float _lateralSpeed;


	private Vector3 _lastPos;
	private Vector3 _forward;
	private Vector3 _right;
	private Vector3 _up;
}
