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

	// Use this for initialization
	private void Start()
	{
		_lastPos = _target.position;
	}


	private void LateUpdate()
	{
		Vector3 targetPos = _target.position;
		Vector3 deltaPos = targetPos - _lastPos;

		Vector3 cameraPos = targetPos;
		cameraPos += _distance.y * _up;
		cameraPos -= _stoppedDistanceRatio*_distance.z*_forward;

		int direction = Math.Sign(Vector3.Dot(deltaPos, _right));

		cameraPos -= Math.Abs( direction ) * (1f - _stoppedDistanceRatio) *_distance.z * _forward;
		cameraPos += direction * _distance.x * _right;

		transform.position = Vector3.Lerp(transform.position, cameraPos, _speed * Time.deltaTime);


		// TODO establecer referencias del scroll 
		// transform.LookAt(boomonPos);


		_lastPos = _target.position;
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

	private enum ScrollDirection
	{
		Stopped = 0,
		Left = -1,
		Right = 1	
	}

	[SerializeField] private Transform _target;
	[SerializeField, Range(1f, 20f)] private float _speed;
	[SerializeField, Range(1f, 20f)] private float _rotationSpeed;
	[SerializeField] private Vector3 _distance;
	[SerializeField, Range(0f, 1f)] private float _stoppedDistanceRatio;

	private Vector3 _lastPos;
	private Vector3 _forward;
	private Vector3 _right;
	private Vector3 _up;
	

	
}
