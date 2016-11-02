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
				Ragdoll.OnGroundEnter(transform.position);
		}
	}

	public bool IsTeleporting
	{
		get { return Ragdoll.IsTeleporting; }
	}


	#endregion

	//================================================================

	#region Public Methods

	public override void Throw(Vector3 applyPosition, Vector3 velocity)
	{
		base.Throw(applyPosition, velocity);
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
		IsGrounded =  Rigid.velocity.sqrMagnitude < Ragdoll.GroundParams.StopVelocityMaxSqr 
			&& (_groundTimer -= Time.fixedDeltaTime) < 0f;
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

	#endregion

	//========================================================================

	#region Private Fields
	
	private float _groundTimer;
	private bool _isGrounded;

	#endregion

	
}


