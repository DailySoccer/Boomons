using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class RagdollPelvis : RigidThrower, ITeleportable
{
	#region Public Fields

	public bool IsGrounded
	{
		get { return _isGrounded; }
		private set
		{
			if (value == _isGrounded)
				return;
			_isGrounded = value;

			if (value)
				OnGroundEnter();
		}
	}

	public bool IsTeleporting { get { return _ragdoll.IsTeleporting;  } }
	

	#endregion

	//================================================================

	#region Public Methods

	public void Init(Ragdoll ragdoll)
	{					
		_ragdoll = ragdoll;
	}

	public override void Throw(Vector3 velocity, Vector3? applyPosition = null)
	{
		base.Throw(velocity, applyPosition);
		_groundTimer = _ragdoll.Setup.TimeoutSecs;
	}

	public void TeleportTo(Teleport target)
	{
		_ragdoll.TeleportTo(target);
	}

	#endregion

	//================================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();

		_gravityDir = Physics.gravity.normalized;
	}

	protected override void OnDestroy()
	{
		_ragdoll = null;
		base.OnDestroy();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		IsGrounded = false;
	}


	private void FixedUpdate()
	{
		if(_ragdoll.ReferenceSystem != null)
			Rigid.MovePosition( _ragdoll.ReferenceSystem.ProjectOnPlane(Rigid.position) );

		//if (Rigid.velocity.sqrMagnitude < Ragdoll.GroundParams.StopVelocityMaxSqr)
		//	IsGrounded = (_groundTimer -= Time.fixedDeltaTime) < 0f;
		//else
		//	_groundTimer = Ragdoll.GroundParams.Timeout;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody == null
		    && Vector3.Dot(-_gravityDir, collision.contacts[0].normal) > _ragdoll.Setup.GroundCosine)
		{
			OnGroundEnter();
		}
	}

	// UNDONE FRS 161114 Ya no se están usando capas
	//private void OnCollisionStay(Collision collision)
	//{
	//	if (collision.gameObject.layer == Ragdoll.GroundParams.Layer
	//		&& Rigid.velocity.sqrMagnitude < Ragdoll.GroundParams.StopVelocityMaxSqr)
	//		Ragdoll.OnGroundEnter(collision.contacts[0].point);
	//}

	#endregion

	//========================================================================

	#region Events

	private void OnGroundEnter()
	{
		Rigid.velocity = Vector3.zero;
		_ragdoll.OnGroundEnter(transform.position);
	}

	#endregion

	//========================================================================

	#region Private Fields


	private float _groundTimer;
	private bool _isGrounded;
	private Vector3 _gravityDir;
	private Ragdoll _ragdoll;

	#endregion

	
}


