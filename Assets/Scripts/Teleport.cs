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
		_inputs = new HashSet<GameObject>();

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
		_inputs = null;
		_animator = null;
		_collider = null;
		_exit = null;
		_exitPoint = null;
	}
	

	private void OnTriggerEnter(Collider c)
	{
		GameObject o = c.gameObject;
		if (_inputs.Contains(o))
			return;

		DoTeleport(o);
	}

	private void OnTriggerExit(Collider c)
	{
		_inputs.Remove(c.gameObject);
	}

	#endregion

	//====================================================================================

	#region Events

	#endregion

	//====================================================================================

	#region Private Methods

	private void DoTeleport(GameObject go)
	{
		if (_animator != null)
			_animator.SetTrigger("Teleport");

		_exit.Receive(go, go.transform.position - transform.position);
	}

	private void Receive(GameObject input, Vector3 localPos)
	{
		_inputs.Add(input);
		
		Receive(input.GetComponentInChildren<Rigidbody>(), localPos);
		if (input.tag == _playerTag)
			Receive(input.GetComponent<BoomonController>(), localPos);
	}

	private void Receive(BoomonController boomon, Vector3 localPos)
	{
		if (boomon == null)
			return;

		boomon.TeleportTo(this);
	}

	private void Receive(Rigidbody rigid, Vector3 localPos)
	{
		if (rigid == null)
			return;

		rigid.position = transform.position + localPos;

		Vector3 exitDir = (_exitPoint.position - transform.position).normalized;
		rigid.velocity = Vector3.zero;
		rigid.AddForce(_expulsionForce * exitDir - rigid.velocity, ForceMode.VelocityChange);
	}

	

	#endregion



	//====================================================================================

	#region Private Fields

	private const string ExitPointName = "ExitPoint";

	[SerializeField] private Teleport _exit;
	[SerializeField] private Transform _exitPoint;
	[SerializeField] private bool _isExitRightward;
	[SerializeField] private string _playerTag = "Player";
	[SerializeField, Range(0f, 50f)] private float _expulsionForce = 2f;
	
	private Collider _collider;
	private Animator _animator;
	private HashSet<GameObject> _inputs;

	#endregion
}
