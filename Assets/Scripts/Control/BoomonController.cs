using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class BoomonController : Touchable
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

	
	public Vector3 Position
	{
		get
		{
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
			_stateActions[value].OnStart(lastState);
		}
	}

	public Sense MoveSense
	{
		get { return _moveSense; }
		private set
		{
			if (value == Sense.None)
				transform.forward = _screenDirection;
			else
				transform.forward = (int)value * _moveDirection;

			_animator.SetInteger("Direction", (int)value);
			_moveSense = value;
		}
	}

	#endregion


	//==============================================================

	#region Public Methods

	public void Jump(Vector2 swipeVector, float speedRatio)
	{
		if (CurrentState == State.Jumping || CurrentState == State.Throwing)
			return;

		float jumpDegress = Mathf.Atan(swipeVector.y / Mathf.Abs(swipeVector.x)) * Mathf.Rad2Deg;
		if (!(jumpDegress > _jumpDegreesMin))
			return;

		MoveSense = jumpDegress > _frontJumpDegreesMin ? 
			Sense.None : (Sense) Mathf.Sign(swipeVector.x);

		_jumpStartVelocity = CalculateJumpSpeed(swipeVector, speedRatio);
		CurrentState = State.Jumping;
	}

	

	public void Throw(Vector2 swipeVector, float speedRatio)
	{
		if(CurrentState == State.Jumping || CurrentState == State.Throwing)
			return;

		float throwDegress = Mathf.Atan(swipeVector.y / Mathf.Abs(swipeVector.x)) * Mathf.Rad2Deg;
		if (!(throwDegress > _throwDegreesMin*Mathf.Deg2Rad))
			return;

		CurrentState = State.Throwing;
		_ragdoll.Setup(transform);
		_ragdoll.OnSwipe(null, swipeVector, speedRatio);
	}


	public void GoTo(Vector3 position, Action callback = null)
	{
		_goTo = position;
		if(callback != null)
			_goToCallbacks.Add(callback);

		if(CurrentState != State.Throwing)
			CurrentState = State.CodeDriven;
	}

	#endregion

	//==============================================================


	#region Mono

	protected override void Awake()
	{
		base.Awake();

		Debug.Assert(_moveDirection != Vector3.zero, "BoomonController::Awake>> Move direction not defined!!");

		_goToCallbacks = new List<Action>();
		_stateActions = new Dictionary<State, StateActions>
		{
			{ State.CodeDriven, new StateActions(OnCodeDriveStart, OnCodeDriveEnd, CodeDriveUpdate)},
			{ State.Idle,       new StateActions(OnIdleStart, OnIdleEnd, IdleUpdate)},
			{ State.Moving,     new StateActions(OnMoveStart, OnMoveEnd, MoveUpdate, OnMoveCollision)},
			{ State.Throwing,   new StateActions(OnThrowStart, OnThrowEnd)},
			{ State.Jumping,    new StateActions(OnJumpStart, OnJumpEnd, JumpUpdate, OnJumpCollision)},
			{ State.Tickling,   new StateActions(OnTickleStart, OnTickleEnd)},
		};
		
		_wallLayer = LayerMask.NameToLayer(_wallLayerName);
		_groundLayer = LayerMask.NameToLayer(_groundLayerName);

		_moveDirection.Normalize();
		_screenDirection = Vector3.Cross(transform.up, _moveDirection).normalized;
		_jumpDirection =  Vector3.Cross(_moveDirection, _screenDirection);
		transform.up = _jumpDirection;

		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();

		if (_bipedRoot == null)
			_bipedRoot = GetComponentsInChildren<Transform>()
				.First(t => t.name.Contains("Bip"));

		_bipedOffsetPos = _bipedRoot.position - transform.position;

		var ragdollPrefab = Resources.Load<GameObject>(_ragdollPath);
		_ragdoll = Instantiate(ragdollPrefab).GetComponent<Ragdoll>();
		_ragdoll.GroundEnter += OnRagdollGroundEnter;
	}



	protected override void OnDestroy()
	{
		_goToCallbacks = null;
		_ragdoll.GroundEnter -= OnRagdollGroundEnter;
		_ragdoll = null;
		_bipedRoot = null;
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
		if (go != null || CurrentState != State.Idle)
			return;

		CurrentState = State.Moving;
	}

	public override void OnTapStop(GameObject go, Vector2 position)
	{
		if(CurrentState == State.Moving)
			CurrentState = State.Idle;
	}

	public override void OnTapStay(GameObject go, Vector2 touchPos)
	{
		if (go != null || CurrentState != State.Moving)
			return;

		Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		
//*
		float moveValue = Vector3.Dot(Vector3.right, touchPos - screenPos);
/*/
		float moveValue = 2f * (touchPos - screenPos).x / Screen.width;
/**/
		if (MoveSense == Sense.None 
			|| Mathf.Abs(moveValue) > _senseReversalInchesMin*Screen.dpi) 
			MoveSense = (Sense) Mathf.Sign(moveValue);
	}

	public override void OnDoubleTap(GameObject go, Vector2 position)
	{
		if (CurrentState == State.Idle && go == gameObject)
			CurrentState = State.Tickling;
	}

	public override void OnSwipe(GameObject go, Vector2 swipeVector, float speedRatio)
	{
		if(go == null)
			Jump(swipeVector, speedRatio);
		else if(go == gameObject)
			Throw(swipeVector, speedRatio);
	}

	//---------------------------------------------------------

	private void OnRagdollGroundEnter(Vector3 groundPos)
	{
		transform.position = groundPos;
		CurrentState = State.Idle;
	}

	//----------------------------------------------------------------------------------


	private void OnIdleReady()
	{
		IsTouchEnabled = true;
	}

	private void OnIdleStart(State lastState)
	{
		Debug.Log("BoomonController::OnIdleStart>> Last=" + lastState);

		MoveSense = Sense.None;
		transform.position -= Vector3.Project(transform.position - _idlePos, _screenDirection);

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

	

	private void OnIdleEnd(State nextState)
	{
		Debug.Log("BoomonController::OnIdleEnd>> Next=" + nextState);
		_idlePos = transform.position;
	}
	
	private void IdleUpdate()
	{
		_controller.SimpleMove(Vector3.zero);
	}
	
	//----------------------------------------------------------------------

	private void OnMoveStart(State lastState)
	{
		Debug.Log("BoomonController::OnMoveStart");
		PlayAnimation(_runTriggerName);
	}

	private void OnMoveEnd(State nextState)
	{
		Debug.Log("BoomonController::OnMoveEnd");
	}

	private void MoveUpdate()
	{
		_controller.SimpleMove(_moveSpeedMax*transform.forward);
	}

	private void OnMoveCollision(ControllerColliderHit hit)
	{
		if (hit.gameObject.layer == _wallLayer)
			CurrentState = State.Idle;
	}

	//---------------------------------------------------------------------------------

	private void OnThrowStart(State lastState)
	{
		Debug.Log("BoomonController::OnThrowStart");
		gameObject.SetActive(false);
	}

	private void OnThrowEnd(State nextState)
	{
		Debug.Log("BoomonController::OnThrowEnd");
		gameObject.SetActive(true);
		_idlePos = transform.position;
	}


	//----------------------------------------------------

	private void OnJumpStart(State lastState)
	{
		Debug.Log("BoomonController::OnJumpStart");

		IsTouchEnabled = false;

		PlayAnimation(_jumpTriggerName);
		_isParaboling = false;
	}

	private void OnJumpTakeOff()
	{
		Debug.Log("BoomonController::OnJumpTakeOff");
		_isParaboling = true;

		_controller.Move(_bipedRoot.position - (_idlePos + _bipedOffsetPos));
		_jumpVelocity = _jumpStartVelocity;
	}

	private void OnJumpEnd(State nextState)
	{
		Debug.Log("BoomonController::OnJumpEnd");
		_isParaboling = false;
	}


	private void JumpUpdate()
	{
		if (!_isParaboling)
			return;

		float t = Time.deltaTime;

		_controller.Move(_jumpVelocity * t + .5f * Physics.gravity * t * t);
		_jumpVelocity += Physics.gravity * t;
	}

	private void OnJumpCollision(ControllerColliderHit hit)
	{
		Debug.Log("BoomonController::OnJumpCollision>> " + hit.gameObject.name);

		if (hit.gameObject.layer == _groundLayer) {
			CurrentState = State.Idle;
			return;

		} else if (hit.gameObject.layer == _wallLayer) {
			ReflectJump(hit.normal);
			return;
		}

		float colliderSlope = Mathf.Abs(Vector3.Angle(hit.normal, _jumpDirection));
		if (colliderSlope < _controller.slopeLimit)
			CurrentState = State.Idle;
		else if (Vector3.Dot(hit.normal, _jumpVelocity) < 0f)
			ReflectJump(hit.normal);
	}

	//------------------------------------------------------------------


	private void OnTickleStart(State obj)
	{
		Debug.Log("BoomonController::OnTickleStart");
		CurrentState = State.Idle;
	}

	private void OnTickleEnd(State obj)
	{
		Debug.Log("BoomonController::OnTickleEnd");
	}



	//------------------------------------------------------


	private void OnCodeDriveStart(State obj)
	{
		Debug.Log("BoomonController::OnCodeDriveStart");

		IsTouchEnabled = false;

		if (_goTo.HasValue) {
			float moveSense = Vector3.Dot(_goTo.Value - transform.position, _moveDirection);
			MoveSense = (Sense) Mathf.Sign(moveSense);
		}
	}

	private void OnCodeDriveEnd(State obj)
	{
		Debug.Log("BoomonController::OnCodeDriveEnd");

		foreach (Action c in _goToCallbacks)
			c();

		_goToCallbacks.Clear();
		_goTo = null;
		_idlePos = transform.position;

		IsTouchEnabled = true;
	}
	
	private void CodeDriveUpdate()
	{
		if (!_goTo.HasValue)
			return;

		Vector3 move = _goTo.Value - transform.position;

		if( move.sqrMagnitude < GoToDistanceMinSqr)
			CurrentState = State.Idle;
		else
			_controller.SimpleMove(_moveSpeedMax*(_goTo.Value - transform.position).normalized);
	}




	#endregion


	//============================================================

	#region Private Methods

	private Vector3 CalculateJumpSpeed(Vector2 swipeVector, float speedRatio)
	{
		Vector3 dir = swipeVector.x * _moveDirection
					+ swipeVector.y * _jumpDirection;
		
/*
		return speedRatio * _jumpSpeedMax * dir.normalized;
/*/
		return _jumpSpeedMax * dir.normalized;
/**/
	}


	private void ReflectJump(Vector3 normal)
	{
		_jumpVelocity = _bounciness * Vector3.Reflect(_jumpVelocity, normal);
		_jumpVelocity = Vector3.ProjectOnPlane(_jumpVelocity, _screenDirection);
		MoveSense = (Sense)(-(int)MoveSense);
	}


	private void PlayAnimation(string trigger)
	{
		Debug.Log("BoomonController::PlayAnimation>> " + trigger);
		_animator.SetTrigger(trigger);
	}
	#endregion

	//=======================================================================

	#region Private Fields

	private const float GoToDistanceMinSqr = .5f;

	[SerializeField] private Transform _bipedRoot;
	[SerializeField] private Vector3 _moveDirection;
	private Vector3 _screenDirection;
	private Vector3 _jumpDirection;
	private Vector3 _bipedOffsetPos;

	[SerializeField, Range(0f, 1f)]		private float _bounciness = 0.8f;
	[SerializeField, Range(0.5f, 50f)]	private float _moveSpeedMax = 5f;
	[SerializeField, Range(0f, 50f)]	private float _jumpSpeedMax = 10f;
	[SerializeField, Range(0f, 90f)]	private float _jumpDegreesMin;
	[SerializeField, Range(0f, 90f)]	private float _frontJumpDegreesMin = 80f;
	[SerializeField, Range(0f, 90f)]	private float _throwDegreesMin;
	[SerializeField, Range(0f, 10f)]	private float _senseReversalInchesMin = 1;

	[SerializeField] private string _idleTriggerName = "Idle";
	[SerializeField] private string _runTriggerName = "Run";
	[SerializeField] private string _jumpTriggerName = "Jump";
	[SerializeField] private string _landTriggerName = "Land";
	[SerializeField] private string _standUpTriggerName = "StandUp";
	[SerializeField] private string _ticklesTriggerName = "Tickles";

	[SerializeField] private string _ragdollPath = "Prefabs/Ragdoll.prefab";
	[SerializeField] private string _wallLayerName = "Wall";
	[SerializeField] private string _groundLayerName = "Ground";

	private Animator _animator;
	private CharacterController _controller;
	private Ragdoll _ragdoll;
	
	
	private Vector3 _idlePos;
	private Vector3 _jumpVelocity;
	private Vector3 _jumpStartVelocity;
	private Vector3? _goTo;
	
	private int _wallLayer;
	private int _groundLayer;
	private bool _isParaboling;

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

	private Dictionary<State, StateActions> _stateActions;
	private List<Action> _goToCallbacks;


	#endregion


}
