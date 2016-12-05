using System;
using UnityEngine;

public class ScrollFollower : MonoBehaviour
{

	#region Public Fields


	[Serializable]
	public class Setup
	{
		public Vector3 Distance { get { return _distance; } }
		public float StopDistanceRatio { get { return _stopDistanceRatio; } }
		public float DepthSpeed { get { return _depthSpeed; } }
		public float LateralSpeed { get { return _lateralSpeed; } }

		public const float DepthSpeedMax = 20f;
		public const float LateralSpeedMax = 20f;
		[SerializeField] private Vector3 _distance = new Vector3(0f, .8f, 4f);
		[SerializeField, Range(0f, 1f)] private float _stopDistanceRatio = .3f;
		[SerializeField, Range(0f, DepthSpeedMax)] private float _depthSpeed = 3f;
		[SerializeField, Range(0f, LateralSpeedMax)] private float _lateralSpeed = 15f;

		public Setup() { }

		public Setup(
			Vector3 distance,
			float stopDistanceRatio = .3f,
			float depthSpeed = 3f,
			float lateralSpeed = 15f)
		{
			_distance = distance;
			_stopDistanceRatio = stopDistanceRatio;
			_depthSpeed = Mathf.Clamp(depthSpeed, 0f, DepthSpeedMax);
			_lateralSpeed = Mathf.Clamp(lateralSpeed, 0f, LateralSpeedMax);
		}

	}

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

	#region
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

		Vector3 p = targetPos;
		p += _currentSetup.Distance.y * RefSystem.Up;
		p += (1f - _currentSetup.StopDistanceRatio) *_currentSetup.Distance.z*RefSystem.ScreenDir;

		int direction = Math.Sign(Vector3.Dot(deltaPos, RefSystem.Right));

		p += Math.Abs( direction ) * _currentSetup.StopDistanceRatio *_currentSetup.Distance.z * RefSystem.ScreenDir;
		p += direction * _currentSetup.Distance.x * RefSystem.Right;

//* // WITH LERP
		Vector3 fwdDeltaPos = Vector3.Project(p - transform.position, RefSystem.ScreenDir);
		Vector3 latPos = p - fwdDeltaPos;

		if(_currentSetup.LateralSpeed < Setup.LateralSpeedMax)
			latPos = Vector3.Lerp(transform.position, latPos , _currentSetup.LateralSpeed * Time.deltaTime);
		if (_currentSetup.DepthSpeed  < Setup.DepthSpeedMax)
			p = Vector3.Lerp(latPos, p, _currentSetup.DepthSpeed*Time.deltaTime);

		transform.position = p;	   
		//transform.forward = (Target.position + .5f * _distance.y * RefSystem.Up - transform.position).normalized;	

/*/ // WITHOUT LERP
		transform.position = pos;
/**/

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


	[SerializeField] private Transform _target;
	[SerializeField] private string _targetTag = "Player";
	[SerializeField] private Setup _currentSetup;  
	protected Setup CurrentSetup {
		get { return _currentSetup; }
		set { _currentSetup = value;  }
	}



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

