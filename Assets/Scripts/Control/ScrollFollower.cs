using System;

using UnityEngine;

public class ScrollFollower : MonoBehaviour
{

	#region Public Fields
	
	public virtual Transform Target 
	{
		get
		{
			if(_target==null ||!_target.gameObject.activeInHierarchy)
			{
				GameObject targetGo = GameObject.FindGameObjectWithTag(_targetTag);
				if (targetGo != null) {
					_target = targetGo.transform;
					_lastPos = _target.position;
				}
			}
			return _target;
		}
	}

	#endregion


	//=================================================

	#region Mono

	protected virtual void Awake()
	{
		
	}

	protected virtual void OnDestroy()
	{
		_target = null;

	}


	private void LateUpdate()
	{
		if (Target == null)
			return;

		Vector3 targetPos = Target.position;
		Vector3 deltaPos = targetPos - _lastPos;

		Vector3 pos = targetPos;
		pos += _distance.y * RefSystem.Up;
		pos += (1f - _stopZoomRatio) *_distance.z*RefSystem.ScreenDir;

		int direction = Math.Sign(Vector3.Dot(deltaPos, RefSystem.Right));

		pos += Math.Abs( direction ) * _stopZoomRatio *_distance.z * RefSystem.ScreenDir;
		pos += direction * _distance.x * RefSystem.Right;

		Vector3 fwdDeltaPos = Vector3.Project(pos - transform.position, RefSystem.ScreenDir);
		transform.position = Vector3.Lerp(transform.position, pos - fwdDeltaPos, _lateralSpeed * Time.deltaTime);;
		transform.position = Vector3.Lerp(transform.position, pos, _depthSpeed * Time.deltaTime);

		transform.forward = (Target.position + .5f * _distance.y * RefSystem.Up - transform.position).normalized;

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

	#region Private Fields

	protected Vector3 Distance{
		get { return _distance;	 }
		set { _distance = value; }
	}

	protected float StopDistanceRatio {
		get { return _stopZoomRatio;  }
		set { _stopZoomRatio = value; }
	}

	protected float DepthSpeed {
		get { return _depthSpeed;	}
		set { _depthSpeed = value;  }
	}

	protected float LateralSpeed {
		get { return _lateralSpeed;	 }
		set { _lateralSpeed = value; }
	}

	[SerializeField] private Transform _target;
	[SerializeField] private string _targetTag = "Player"; 
	[SerializeField] private Vector3 _distance;
	[SerializeField, Range(0f, 1f)] private float _stopZoomRatio;
	[SerializeField, Range(0f, 20f)] private float _depthSpeed;
	[SerializeField, Range(0f, 20f)] private float _lateralSpeed;


	private Vector3 _lastPos;

	private ReferenceSystem _refSystem;
	protected virtual ReferenceSystem  RefSystem
	{
		get {
			return _refSystem ??
			(_refSystem = new ReferenceSystem(transform.position, transform.right) );
		}
	}
	#endregion 

}

