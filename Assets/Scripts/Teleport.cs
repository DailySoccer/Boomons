using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Teleport : MonoBehaviour
{
	#region Public Fields

	public bool IsExpulsing
	{
		get { return _isExpulsing;  }
		private set
		{
			_isExpulsing = value;
			enabled = value;
			IsOpen = !value;
		}
	}

	public bool IsOpen
	{
		get { return _collider.enabled; }
		set { _collider.enabled = value; }
	}
	#endregion

	//=========================================================================

	#region 

	private void Awake()
	{
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
		_expulsedRigid = null;
		_animator = null;
		_collider = null;
		_exit = null;
		_exitPoint = null;
	}

	//private void OnEnable()
	//{
		
	//}

	private void OnDisable()
	{
		_expulsedRigid = null;
	}

	private void Start()
	{
		IsExpulsing = false;
	}

	private void OnTriggerEnter(Collider input)
	{
		if(_animator != null)
			_animator.SetTrigger("Teleport");

		_exit.Receive(input.gameObject);
	}

	private void FixedUpdate()
	{
		IsExpulsing = _expulsedRigid.velocity.sqrMagnitude > StopVelocityMaxSqr;
	}
	
	#endregion

	//====================================================================================

	#region Events

	private void OnBoomonExited()
	{
		IsOpen = true;
	}

	#endregion

	//====================================================================================

	#region Private Methods

	private void Receive(GameObject input)
	{
		input.transform.position = transform.position;

		if (input.tag == _playerTag)
			Receive(input.GetComponent<BoomonController>());
		else
			Receive(input.GetComponentInChildren<Rigidbody>());
	}

	private void Receive(BoomonController boomon)
	{
		if (boomon == null)
			return;

		IsOpen = false;
		boomon.GoTo(_exitPoint.position, OnBoomonExited);
	}

	private void Receive(Rigidbody rigid)
	{
		if (rigid == null)
			return;

		Expulse(rigid);
	}

	private void Expulse(Rigidbody rigid)
	{
		IsExpulsing = true;

		_expulsedRigid = rigid;
		Vector3 exitDir = (_exitPoint.position - transform.position).normalized;
		rigid.AddForce(_expulsionForce * exitDir, ForceMode.VelocityChange);
	}

	#endregion

	//====================================================================================

	#region Private Fields

	private const string ExitPointName = "ExitPoint";
	private const float StopVelocityMaxSqr = 1f;

	[SerializeField] private Teleport _exit;
	[SerializeField] private Transform _exitPoint;
	[SerializeField] private string _playerTag = "Player";
	[SerializeField, Range(0f, 20f)] private float _expulsionForce = 2f;
	
	private Collider _collider;
	private Animator _animator;
	private Rigidbody _expulsedRigid;
	private bool _isExpulsing;

	#endregion
}
