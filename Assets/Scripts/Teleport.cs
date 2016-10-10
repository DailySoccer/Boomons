using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Teleport : MonoBehaviour
{
	#region Public Fields

	public Vector3 ExitPoint
	{
		get { return _exitPoint.position;  }
	}

	public Vector3 Right {
		get {
			return (_isExitRightward ? 1 : -1) 
				* (ExitPoint - transform.position).normalized;
		}
	}

	#endregion

	//=========================================================================

	#region 

	private void Awake()
	{
		_rigids		= new HashSet<Rigidbody>();

		_collider = GetComponent<Collider>();
		_animator = GetComponent<Animator>();
		_collider.isTrigger = true;

		Debug.Assert(_exit != null, "Teleport::Awake>> Exit not defined @ " + name, this);

		if(_exitPoint == null)
			_exitPoint = transform.Find(ExitPointName);

		Debug.Assert(_exitPoint != null, "Teleport::Awake>> Exit point not defined @ " + name, this);
	}

	private void OnDestroy()
	{
		_rigids = null;

		_animator = null;
		_collider = null;
		_exit = null;
		_exitPoint = null;
	}
	

	private void OnTriggerEnter(Collider c)
	{
		Debug.Assert(_rigids != null, "Teleport::OnTriggerEnter>> Rigid Set not defined!");

		if(_rigids.Contains(c.attachedRigidbody)
		|| c.GetComponent<ITeleportable>().IsTeleporting)
			return;

		DoTeleport(c.gameObject);
	}

	private void OnTriggerExit(Collider c)
	{
		_rigids.Remove(c.attachedRigidbody);
	}

	#endregion

	//====================================================================================

	#region Events

	#endregion

	//====================================================================================

	#region Private Methods

	private void DoTeleport(GameObject go)
	{
		Debug.Log("Teleport::DoTeleport>> " + go.name, this);

		if (_animator != null)
			_animator.SetTrigger("Teleport");

		_exit.Receive(go, go.transform.position - transform.position);
	}

	private void Receive(GameObject input, Vector3 localPos)
	{
		if (!Receive(input.GetComponent<ITeleportable>()))
			Receive(input.GetComponentInChildren<Rigidbody>(), localPos);
	}

	private bool Receive(ITeleportable teleportable)
	{
		if (teleportable == null)
			return false;

		Debug.Log("Teleport::Receive>> Teleportable", this);

		teleportable.TeleportTo(this);
		return true;
	}

	private bool Receive(Rigidbody rigid, Vector3 localPos)
	{
		if (rigid == null)
			return false;

		Debug.Log("Teleport::Receive>> Rigid=" + rigid.name, this);

		_rigids.Add(rigid);

		rigid.position = transform.position + localPos;

		Vector3 exitDir = (_exitPoint.position - transform.position).normalized;
		rigid.velocity = Vector3.zero;
		rigid.AddForce(_expulsionForce * exitDir - rigid.velocity, ForceMode.VelocityChange);
		return true;
	}

	
	

	#endregion



	//====================================================================================

	#region Private Fields

	private const string ExitPointName = "ExitPoint";

	[SerializeField] private Teleport _exit;
	[SerializeField] private Transform _exitPoint;
	[SerializeField] private bool _isExitRightward;
	[SerializeField, Range(0f, 50f)] private float _expulsionForce = 2f;
	
	private Collider _collider;
	private Animator _animator;
	private HashSet<Rigidbody> _rigids;

	#endregion
}
