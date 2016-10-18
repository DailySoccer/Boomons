using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Animator), typeof(CharacterController), typeof(FacialAnimator))]
public class BoomonController : Touchable, ITeleportable
{
	#region Public Fields

	
	public enum State 
	{
		CodeDriven = -1,
		Idle = 0,
		Moving = 1,
		Throwing = 2,
		Jumping = 3,
		Tickling = 4,
	};

	public enum Sense
	{
		Left = -1,
		None = 0,
		Right = 1
	}

	
	public Vector3 Position {
		get {
			return CurrentState == State.Throwing ?
				_ragdoll.Position : transform.position;
		}
	}
	
	public State CurrentState
	{
		get { return _currentState; }
		private set
		{
			if (value == _currentState)
				return;

			State lastState = _currentState;
			_stateActions[lastState].OnEnd(value);
			_currentState = value;
			_stateActions[_currentState].OnStart(lastState);
		}
	}

	public Sense MoveSense
	{
		get { return _moveSense; }
		private set
		{
			if (value == Sense.None)
				transform.forward = _refSystem.ScreenDir;
			else
				transform.forward = (int)value * _refSystem.Right;

			_animator.SetInteger("Direction", (int)value);
			_moveSense = value;
		}
	}

	public bool IsTeleporting { get; private set; }
	#endregion
	
	//==============================================================

	#region Public Methods

	public void Move(Vector2 touchPos)
	{
		Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
//*
		float moveValue = Vector3.Dot(Vector3.right, touchPos - screenPos);
/*/
		float moveValue = 2f * (touchPos - screenPos).x / Screen.width;
/**/
		if (MoveSense == Sense.None || Mathf.Abs(moveValue) > _senseReversalDistMin)
			MoveSense = (Sense)Mathf.Sign(moveValue);

		_velocity = _moveSpeedMax * transform.forward;
		CurrentState = State.Moving;
	}

	//public void Jump(Vector2 swipeVector, float speedRatio)
	//{
	//	if (CurrentState == State.Jumping || CurrentState == State.Throwing)
	//		return;

	//	float jumpDegress = Mathf.Atan(swipeVector.y / Mathf.Abs(swipeVector.x)) * Mathf.Rad2Deg;
	//	if (!(jumpDegress > _jumpDegreesMin))
	//		return;

	//	MoveSense = jumpDegress > _frontJumpDegreesMin ?
	//		Sense.None : (Sense)Mathf.Sign(swipeVector.x);

	//	_jumpStartVelocity = CalculateJumpSpeed(swipeVector, speedRatio);
	//	CurrentState = State.Jumping;
	//}

	public void Throw(Vector2 swipePos, Vector2 swipeVector, float speedRatio)
	{
		if(CurrentState == State.Throwing || CurrentState == State.Jumping)
			return;

		float throwDegress = Mathf.Atan(swipeVector.y / Mathf.Abs(swipeVector.x)) * Mathf.Rad2Deg;
		if (!(throwDegress > _throwDegreesMin*Mathf.Deg2Rad))
			return;

		CurrentState = State.Throwing;
		_ragdoll.Setup(transform);
		_ragdoll.OnSwipe(null, swipePos, swipeVector, speedRatio);
	}

	public void TeleportTo(Teleport target)
	{
		IsTeleporting = true;
		CurrentState = State.CodeDriven;

		transform.position = target.transform.position;
		_refSystem = new ReferenceSystem(transform.position, target.Right);

		GoTo( target.ExitPoint );
	}

	public void GoTo(Vector3 position, Action callback = null)
	{
		_drivenTarget = position;
		if(callback != null)
			_goToCallbacks.Add(callback);

		Vector3 move = _drivenTarget - transform.position;
		float moveSense = Vector3.Dot(move, _refSystem.Right);
		MoveSense = (Sense) Mathf.Sign(moveSense);

		_velocity = _moveSpeedMax * transform.forward;
		CurrentState = State.CodeDriven;
	}

	#endregion

	//==============================================================
	
	#region Mono

	protected override void Awake()
	{
		base.Awake();

		Debug.Assert(_right != Vector3.zero, "BoomonController::Awake>> Right move direction not defined!!");

		_goToCallbacks = new List<Action>();
		_stateActions = new Dictionary<State, StateActions>
		{
			{ State.CodeDriven, new StateActions(OnCodeDriveStart, OnCodeDriveEnd, CodeDriveUpdate)},
			{ State.Idle,       new StateActions(OnIdleStart, OnIdleEnd, IdleUpdate)},
			{ State.Moving,     new StateActions(OnMoveStart, OnMoveEnd, MoveUpdate, OnMoveCollision)},
			{ State.Throwing,   new StateActions(OnThrowStart, OnThrowEnd)},
			//{ State.Jumping,    new StateActions(OnJumpStart, OnJumpEnd, JumpUpdate, OnJumpCollision)},
			{ State.Tickling,   new StateActions(OnTickleStart, OnTickleEnd)},
		};
		

		_refSystem = new ReferenceSystem(transform.position, _right);
		transform.up = _refSystem.JumpDir;

		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();
		_face = GetComponent<FacialAnimator>();
		_groundSlopeCosine = Mathf.Cos(_controller.slopeLimit * Mathf.Deg2Rad);

		//if (_bipedRoot == null)
		//	_bipedRoot = GetComponentsInChildren<Transform>()
		//		.First(t => t.name.Contains("Bip"));

		//_bipedOffsetPos = _bipedRoot.position - transform.position;

		var ragdollPrefab = Resources.Load<GameObject>(_ragdollPath);
		_ragdoll = Instantiate(ragdollPrefab).GetComponent<BoomonRagdoll>();
		_ragdoll.GroundEnter += OnRagdollGroundEnter;
	}



	protected override void OnDestroy()
	{
		_goToCallbacks = null;
		_ragdoll.GroundEnter -= OnRagdollGroundEnter;

		_face = null;
		_ragdoll = null;
		//_bipedRoot = null;
		_animator = null;
		_controller = null;

		base.OnDestroy();
	}



	private void Start()
	{
		MoveSense = Sense.None;
		CurrentState = State.Idle;
	}

	/// <summary>
	/// 
	/// </summary>
	private void Update()
	{
		Action update = _stateActions[CurrentState].Update;
		if (update != null)
			update();
	}


	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Action<ControllerColliderHit> collisionEnter = _stateActions[CurrentState].OnCollisionEnter;
		if (collisionEnter != null)
			collisionEnter(hit);
	}

	

	#endregion
	
	//==============================================================

	#region Events


	public override void OnTapStart(GameObject go, Vector2 touchPos)
	{
		if (go == null && CurrentState == State.Idle)
			CurrentState = State.Moving;
	}

	public override void OnTapStop(GameObject go, Vector2 position)
	{
		if(CurrentState == State.Moving)
			CurrentState = State.Idle;
	}

	public override void OnTapStay(GameObject go, Vector2 touchPos)
	{
		if (go == null && CurrentState == State.Moving)
			Move(touchPos);
	}

	public override void OnDoubleTap(GameObject go, Vector2 position)
	{
		if (CurrentState == State.Idle && go == gameObject)
			CurrentState = State.Tickling;
	}

	public override void OnSwipe(GameObject go, Vector2 position, Vector2 direction, float speedRatio)
	{
		Log("OnSwipe");
		//if (go != gameObject)
		//{
		//	if (go != null)
		//		return;

		//	Vector2 myScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		//	if ((myScreenPos - position).sqrMagnitude > _inchesSqrMax)
		//		return;
		//}
		if (go == gameObject)
			Throw(position, direction, speedRatio);
	}

	//---------------------------------------------------------

	private void OnRagdollGroundEnter(Vector3 groundPos)
	{
		transform.position = groundPos;
		CurrentState = State.Idle;
	}

	//----------------------------------------------------------------------------------

	/// <summary>
	/// Animation Event
	/// Indicates that Idle animation is playing.
	/// </summary>
	private void OnIdleReady()
	{
		IsTouchEnabled = true;
	}

	private void OnIdleStart(State lastState)
	{
		Log("OnIdleStart", "Last=" + lastState);

		_velocity = Vector3.zero;
		MoveSense = Sense.None;
		transform.position = _refSystem.FixInPlane(transform.position);

		switch (lastState)
		{
			case State.Throwing:
				PlayAnimation(_standUpTriggerName);
				break;
			case State.Jumping:
				PlayAnimation(_landTriggerName);
				break;
			case State.Tickling:
				PlayAnimation(_ticklesTriggerName);
				break;
			default:
				PlayAnimation(_idleTriggerName);
				break;
		}
	}
	

	/// <summary>
	/// 
	/// </summary>
	/// <param name="nextState"></param>
	private void OnIdleEnd(State nextState)
	{
		Log("OnIdleEnd" , "Next=" + nextState);

		IsTouchEnabled = nextState == State.Tickling
				      || nextState == State.Moving;
	}
	
	private void IdleUpdate()
	{
		_controller.SimpleMove(Vector3.zero);
	}
	
	//----------------------------------------------------------------------

	private void OnMoveStart(State lastState)
	{
		Log("OnMoveStart");
		PlayAnimation(_runTriggerName);
	}

	private void OnMoveEnd(State nextState)
	{
		Log("OnMoveEnd");
		_velocity = Vector3.zero;
	}

	private void MoveUpdate()
	{
		_controller.SimpleMove(_velocity);
	}

	private void OnMoveCollision(ControllerColliderHit hit)
	{
		//Log("OnMoveCollision", hit.gameObject.name);

		Rigidbody rigid = hit.collider.attachedRigidbody;

		if(rigid != null) 
			Push(rigid);
		else if (Vector3.Dot(hit.normal, _refSystem.JumpDir) < _groundSlopeCosine)
			CurrentState = State.Idle;
	}

	//---------------------------------------------------------------------------------

	private void OnThrowStart(State lastState)
	{
		Log("OnThrowStart");
		gameObject.SetActive(false);
		_ragdoll.gameObject.SetActive(true);
	}

	private void OnThrowEnd(State nextState)
	{
		Log("OnThrowEnd");

		_face.Reset();
		_ragdoll.gameObject.SetActive(false);
		gameObject.SetActive(true);
	}


	//----------------------------------------------------

	//private void OnJumpStart(State lastState)
	//{
	//	Log("OnJumpStart");

	//	_isParaboling = false;
	//}

	//private void OnJumpTakeOff()
	//{
	//	Log("OnJumpTakeOff");
	//	_isParaboling = true;

	//	_controller.Move(_bipedRoot.position - (_refSystem.PlanePoint + _bipedOffsetPos));
	//	_velocity = _jumpStartVelocity;
	//}

	//private void OnJumpEnd(State nextState)
	//{
	//	Log("OnJumpEnd");
	//	_isParaboling = false;
	//	_velocity = Vector3.zero;
	//}


	//private void JumpUpdate()
	//{
	//	if (!_isParaboling)
	//		return;

	//	float t = Time.deltaTime;

	//	_controller.Move(_velocity * t + .5f * Physics.gravity * t * t);
	//	_velocity += Physics.gravity * t;
	//}

	//private void OnJumpCollision(ControllerColliderHit hit)
	//{
	//	Log("OnJumpCollision", hit.gameObject.name);

	//	float slopeCosine = Vector3.Dot(hit.normal, _refSystem.JumpDir);

	//	if (slopeCosine > _groundSlopeCosine) {
	//		CurrentState = State.Idle;
	//		return;
	//	}
		
	//	Rigidbody rigid = hit.collider.attachedRigidbody;
	//	if (rigid != null)
	//		Push(rigid);
	//	else
	//		Bounce(hit.normal);
		
	
	//	//float colliderSlope = Mathf.Abs(Vector3.Angle(hit.normal, _jumpDirection));
	//	//if (colliderSlope < _controller.slopeLimit) {
	//	//	CurrentState = State.Idle;

	//	//} else if (Vector3.Dot(hit.normal, _velocity) < 0f) {
	//	//	Bounce(hit.normal);
			
	//	//}
	//}

	//------------------------------------------------------------------


	private void OnTickleStart(State obj)
	{
		Log("OnTickleStart");
		CurrentState = State.Idle;
	}

	private void OnTickleEnd(State obj)
	{
		Log("OnTickleEnd");
	}

	//-------------------------------------------------------------------


	private void OnCodeDriveStart(State obj)
	{
		Log("OnCodeDriveStart");
		PlayAnimation(_runTriggerName);
	}

	private void OnCodeDriveEnd(State obj)
	{
		Log("OnCodeDriveEnd");

		IsTeleporting = false;

		foreach (Action c in _goToCallbacks)
			c();
		_goToCallbacks.Clear();
	}
	
	private void CodeDriveUpdate()
	{
		bool hasReachedTarget = Vector3.Dot(_velocity, transform.position - _drivenTarget) > 0f;
		if (hasReachedTarget)
			CurrentState = State.Idle;
		else
			_controller.SimpleMove(_velocity);
	}


	#endregion
	
	//============================================================

	#region Private Methods

//	private Vector3 CalculateJumpSpeed(Vector2 swipeVector, float speedRatio)
//	{
//		Vector3 dir = swipeVector.x * _refSystem.Right
//					+ swipeVector.y * _refSystem.JumpDir;
		
///*
//		return speedRatio * _jumpSpeedMax * dir.normalized;
///*/
//		return _jumpSpeedMax * dir.normalized;
///**/
//	}


	private void Bounce(Vector3 normal)
	{
		_velocity = _bounciness * Vector3.Reflect(_velocity, normal);
		_velocity = Vector3.ProjectOnPlane(_velocity, _refSystem.ScreenDir);

		float sense = Vector3.Dot( _velocity, _right);
		MoveSense = (Sense) Mathf.Sign(sense);
	}


	private void Push(Rigidbody rigid)
	{
		if (rigid == null || rigid.isKinematic)
			return;

		rigid.AddForce( (1f - _bounciness) * _pushMass * _velocity, ForceMode.Impulse);
		_velocity *= _bounciness;

		var item = rigid.GetComponent<ItemActivator>();
		if (item != null)
			item.Activate();
	}

	

	private void PlayAnimation(string trigger)
	{
		Log("PlayAnimation", trigger);
		_animator.SetTrigger(trigger);
	}


	[Conditional("DEBUG_BOOMON")]
	protected void Log(string method, object msg = null, UnityEngine.Object context = null)
	{
		string log = "BoomonController::" + method;
		if (msg != null && msg.ToString() != string.Empty)
			log += ">> " + msg;

		Debug.Log("<b><color=cyan>" + log + "</color></b>", context ?? this);
	}
	#endregion

	//=======================================================================

	#region Private Fields

	//[SerializeField] private Transform _bipedRoot;
	[SerializeField] private Vector3 _right;
	private Vector3 _bipedOffsetPos;

	
	[SerializeField, Range(0f, 1f)]		private float _bounciness = 0.8f;
	[SerializeField, Range(0f, 20f)]	private float _pushMass = 5f;
   	[SerializeField, Range(0.5f, 50f)]	private float _moveSpeedMax = 5f;
	//[SerializeField, Range(0f, 50f)]	private float _jumpSpeedMax = 10f;
	//[SerializeField, Range(0f, 90f)]	private float _jumpDegreesMin;
	//[SerializeField, Range(0f, 90f)]	private float _frontJumpDegreesMin = 80f;
	[SerializeField, Range(0f, 90f)]	private float _throwDegreesMin;
	[SerializeField, Range(0f, 10f)]	private float _senseReversalDistMin = 1;

	[SerializeField] private string _idleTriggerName = "Idle";
	[SerializeField] private string _runTriggerName = "Run";
	[SerializeField] private string _jumpTriggerName = "Jump";
	[SerializeField] private string _landTriggerName = "Land";
	[SerializeField] private string _standUpTriggerName = "StandUp";
	[SerializeField] private string _ticklesTriggerName = "Tickles";

	[SerializeField] private string _ragdollPath = "Prefabs/Ragdoll.prefab";
	

	private Animator _animator;
	private CharacterController _controller;
	private BoomonRagdoll _ragdoll;
	
	private Vector3 _velocity;
	private Vector3 _jumpStartVelocity;
	
	private bool _isParaboling;
	private float _groundSlopeCosine;

	private Sense _moveSense;
	private State _currentState = State.Moving;

	private struct StateActions
	{
		public readonly Action<State> OnStart;
		public readonly Action<State> OnEnd;
		public readonly Action Update;
		public readonly Action<ControllerColliderHit> OnCollisionEnter;
		
		public StateActions(
			Action<State> onStart, 
			Action<State> onEnd, 
			Action update = null,
			Action<ControllerColliderHit> onCollisionEnter = null)
		{
			OnStart = onStart;
			OnEnd = onEnd;
			Update = update;
			OnCollisionEnter = onCollisionEnter;
		}
	}


	private struct ReferenceSystem
	{
		public readonly Vector3 PlanePoint;
		public readonly Vector3 Right;
		public readonly Vector3 JumpDir;
		public readonly Vector3 ScreenDir;

		public ReferenceSystem(Vector3 point, Vector3 right)
		{
			PlanePoint = point;

			right.Normalize();
			JumpDir = -Physics.gravity.normalized;
			ScreenDir = Vector3.Cross(JumpDir, right);
			Right = Vector3.Cross(ScreenDir, JumpDir);
		}

		public Vector3 FixInPlane(Vector3 position)
		{
			return position - Vector3.Project(position - PlanePoint, ScreenDir);
		}
	}


	private ReferenceSystem _refSystem;


	private Dictionary<State, StateActions> _stateActions;
	private List<Action> _goToCallbacks;
	private Vector3 _drivenTarget;
	private FacialAnimator _face
		;

	#endregion


}
