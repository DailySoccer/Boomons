using System;
using System.Collections;
using UnityEngine;


public class ProximityDetector : MonoBehaviour
{
	#region Public Fields

	public virtual event Action TargetEnter;
	public virtual event Action TargetExit;

	public virtual bool IsTargetNearby {
		get { return _isTargetNearby; }
		private set {
			if(value == _isTargetNearby)
				return;

			_isTargetNearby = value; 
			if(value) OnTargetEnter();
			else	  OnTargetExit();
		}
	}

	#endregion


	//======================================================

	#region Mono

	protected virtual void Awake()
	{
		_game = MetaManager.Instance.Get<GameManager>();
	}  
	
	protected virtual void OnDestroy()
	{
		_game = null;
		_proximityTarget = null;
	}

	private IEnumerator DistanceChecker()
	{
		for(;;) {
			IsTargetNearby = IsWithinProximityRadius(_proximityTarget);
			yield return ProximityCheckYield;
		}
	}

	#endregion
	
	//======================================================================

	//======================================================================

	#region Events

	protected virtual void OnTargetEnter()
	{
		var e = TargetEnter;
		if(e != null)
			e();
	}

	protected virtual void OnTargetExit()
	{
		var e = TargetExit;
		if(e != null)
			e();
	}


	#endregion

	//======================================================================

	#region Private Methods

	private bool IsWithinProximityRadius(Transform target)
	{
		if(target == null)
			return false;

		Vector3 dist = target.position - transform.position;
		dist = _game.ReferenceSystem.ProjectOnPlane(dist, false);
		float distSqr = Vector3.SqrMagnitude(dist);

		return distSqr < _proximityRadius * _proximityRadius;
	}

	#endregion

	//====================================================

	#region Private Fields

	[SerializeField, Range(0f, 10f)] private float _proximityRadius = 4f;

	protected GameManager Game { get { return _game;  } }
	protected virtual Transform ProximityTarget 
	{
		get { return _proximityTarget; }
		set
		{
			StopAllCoroutines();
			_proximityTarget = value;
			if(_proximityTarget != null)
				StartCoroutine(DistanceChecker());
		}
	}


	public readonly WaitForSeconds ProximityCheckYield = new WaitForSeconds(.5f);

	private bool _isTargetNearby;
	private Transform _proximityTarget;
	private GameManager _game;

	#endregion
}

