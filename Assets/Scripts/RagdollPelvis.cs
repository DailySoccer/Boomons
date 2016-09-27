using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RagdollPelvis : MonoBehaviour
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

	private void Awake()
	{
		_rigid = GetComponent<Rigidbody>();
	}

	private void OnDestroy()
	{
		_rigid = null;
		Ragdoll = null;
	}

	private void OnEnable()
	{
		IsGrounded = false;
		_groundTimer = _groundTimeout;
	}


	private void FixedUpdate()
	{
		IsGrounded =  _rigid.velocity.sqrMagnitude < Ragdoll.StopVelocityMaxSqr 
			&& (_groundTimer -= Time.fixedDeltaTime) < 0f;
			
	}

	private void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.layer == Ragdoll.GroundLayer
			&& _rigid.velocity.sqrMagnitude < Ragdoll.StopVelocityMaxSqr)
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
	private Rigidbody _rigid;

	#endregion


}
