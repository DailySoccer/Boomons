using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RagdollPelvis : RigidThrower
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
	#endregion

	//================================================================

	#region Public Methods
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
		_groundTimer = _groundTimeout;
	}


	private void FixedUpdate()
	{
		IsGrounded =  Rigid.velocity.sqrMagnitude < Ragdoll.StopVelocityMaxSqr 
			&& (_groundTimer -= Time.fixedDeltaTime) < 0f;
			
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.layer == Ragdoll.GroundLayer
			&& Rigid.velocity.sqrMagnitude < Ragdoll.StopVelocityMaxSqr)
			Ragdoll.OnGroundEnter(collisionInfo.contacts[0].point);
	}

	#endregion

	//========================================================================

	#region Events
	#endregion

	//========================================================================

	#region Private Fields
	
	[SerializeField, Range(0.1f, 5f)] private float _groundTimeout = 1f;
	private float _groundTimer;
	private bool _isGrounded;

	#endregion


}
