using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RagdollPelvis : RigidThrower, ITeleportable
{
	#region Public Fields
	
	public Ragdoll Ragdoll { get; set; }
 
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

	
	public bool IsTeleporting
	{
		get { return Ragdoll.IsTeleporting; }
	}


	#endregion

	//================================================================

	#region Public Methods

	public void Setup(ReferenceSystem refSystem = null)
	{
		_refSystem = refSystem;
	}

	public override void Throw(Vector3 velocity, Vector3? applyPosition = null)
	{		
		base.Throw(velocity, applyPosition);
		_groundTimer = Ragdoll.GroundParams.Timeout;
	}

	public void TeleportTo(Teleport target)
	{
		Ragdoll.TeleportTo(target);
	}

	

	#endregion

	//================================================================

	#region Mono

	protected override void OnDestroy()
	{
		Ragdoll = null;
		base.OnDestroy();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		IsGrounded = false;
	}


	private void FixedUpdate()
	{
		if (_refSystem != null)
			transform.position = _refSystem.ProjectOnPlane(transform.position);

		if (Rigid.velocity.sqrMagnitude < Ragdoll.GroundParams.StopVelocityMaxSqr)
			IsGrounded = (_groundTimer -= Time.fixedDeltaTime) < 0f;
		else
			_groundTimer = Ragdoll.GroundParams.Timeout;
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.layer == Ragdoll.GroundParams.Layer
			&& Rigid.velocity.sqrMagnitude < Ragdoll.GroundParams.StopVelocityMaxSqr)
			Ragdoll.OnGroundEnter(collisionInfo.contacts[0].point);
	}

	#endregion

	//========================================================================

	#region Events

	private void OnGroundEnter()
	{
		Rigid.velocity = Vector3.zero;
		Ragdoll.OnGroundEnter(transform.position);
	}

	#endregion

	//========================================================================

	#region Private Fields

	private float _groundTimer;
	private bool _isGrounded;
	private ReferenceSystem _refSystem;

	#endregion


}


