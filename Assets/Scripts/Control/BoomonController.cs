using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class BoomonController : Touchable
{
	#region Public Fields

	public Vector3 Position {
		get {
			return CurrentState == State.Throwing ? 
				_ragdoll.Position : transform.position;
		} 
	}

	public enum State 
	{
		Idle = 0,
		Moving = 1,
		Throwing = 2,
		Jumping = 3,
	};



	public State CurrentState
	{
		get { return _currentState; }
		private set
		{
			if (value == _currentState)
				return;

			State lastState = _currentState;
			_currentState = value;

			_stateActions[lastState].OnEnd(value);
			_stateActions[value].OnStart(lastState);
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
		if (!(jumpDegress > _jumpDegreesMin * Mathf.Deg2Rad))
			return;

		_jumpSense = jumpDegress > _frontJumpDegreesMin ? 
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

	#endregion

	//==============================================================


	#region Mono

	protected override void Awake()
	{
		base.Awake();

		Debug.Assert(_moveDirection != Vector3.zero, "BoomonController::Awake>> Move direction not defined!!");
		
		_stateActions = new Dictionary<State, StateActions>
		{
			{ State.Idle,       new StateActions(OnIdleStart, OnIdleEnd, IdleUpdate)},
			{ State.Moving,     new StateActions(OnMoveStart, OnMoveEnd, MoveUpdate)},
			{ State.Throwing,   new StateActions(OnThrowStart, OnThrowEnd, ThrowUpdate)},
			{ State.Jumping,    new StateActions(OnJumpStart, OnJumpEnd, JumpUpdate)},
		};

		_wallLayer = LayerMask.NameToLayer(_wallLayerName);
		
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
		_ragdoll.GroundEnter -= OnRagdollGroundEnter;
		_ragdoll = null;
		_bipedRoot = null;
		_animator = null;
		_controller = null;

		base.OnDestroy();
	}



	private void Start()
	{
		CurrentState = State.Idle;
	}

	/// <summary>
	/// 
	/// </summary>
	private void Update()
	{
		_stateActions[CurrentState].Update();
	}


	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(CurrentState == State.Moving && hit.gameObject.layer == _wallLayer)
			CurrentState = State.Idle;
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
		float moveValue = Mathf.Sign(Vector3.Dot(Vector3.right, touchPos - screenPos));
		/*/
		float moveValue = 2f * (touchPos - screenPos).x / Screen.width;
		/**/
	
		transform.forward = moveValue*_moveDirection;
		_moveRatio = Math.Abs(moveValue);
	}

	public override void OnDoubleTap(GameObject go, Vector2 position)
	{
		if(go == gameObject)
			_animator.SetTrigger(_ticklesTriggerName);
	}

	public override void OnSwipe(GameObject go, Vector2 swipeVector, float speedRatio)
	{
		if(go == null)
			Jump(swipeVector, speedRatio);
		else if(go == gameObject)
			Throw(swipeVector, speedRatio);
	}


	//----------------------------------------------------------------------------------


	private void OnIdleStart(State lastState)
	{
		_moveRatio = 0f;
		transform.forward = _screenDirection;
		transform.position -= Vector3.Project(transform.position - _idlePos, _screenDirection);
		
		if(lastState == State.Throwing)
			_animator.SetTrigger(_standUpTriggerName);
		else
			_animator.SetTrigger(_idleTriggerName);
	}

	private void OnIdleEnd(State nextState)
	{
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
		
		_animator.SetTrigger(_runTriggerName);
	}

	private void OnMoveEnd(State nextState)
	{
		Debug.Log("BoomonController::OnMoveEnd");
	}

	private void MoveUpdate()
	{
		_controller.SimpleMove(_moveRatio*_moveSpeedMax*transform.forward);
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
		_animator.SetTrigger(_standUpTriggerName);
	}

	private void ThrowUpdate()
	{
	}

	//----------------------------------------------------

	private void OnJumpStart(State lastState)
	{
		Debug.Log("BoomonController::OnJumpStart");

		if(_jumpSense == Sense.None)
			transform.forward = _screenDirection;
		else
			transform.forward = (int) _jumpSense * _moveDirection;

		_animator.SetInteger("Direction", (int) _jumpSense);
		_animator.SetTrigger(_jumpTriggerName);

		_isParaboling = false;
	}

	private void OnJumpEnd(State nextState)
	{
		Debug.Log("BoomonController::OnJumpEnd");
		CurrentState = State.Idle;
	}

	private void OnJumpTakeOff()
	{
		Debug.Log("BoomonController::OnJumpTakeOff");
		_isParaboling = true;

		transform.position += _bipedRoot.position - (_idlePos + _bipedOffsetPos);
		_jumpVelocity = _jumpStartVelocity;
	}

	private void OnJumpLand()
	{
		Debug.Log("BoomonController::OnJumpLand");

		_isParaboling = false;

		transform.position -= Vector3.Project(transform.position - _idlePos, _jumpDirection);
		transform.forward = _screenDirection;

		_animator.SetTrigger(_landTriggerName);
	}


	private void JumpUpdate()
	{
		if (!_isParaboling)
			return;

		float t = Time.deltaTime;
		transform.position += _jumpVelocity*t + .5f * Physics.gravity*t*t;
		_jumpVelocity += Physics.gravity * t;

		if(_jumpVelocity.sqrMagnitude > _jumpStartVelocity.sqrMagnitude)
			OnJumpLand();
	}

	

	//---------------------------------------------------------

	private void OnRagdollGroundEnter(Vector3 groundPos)
	{
		transform.position = groundPos;
		CurrentState = State.Idle;
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

	#endregion

	//=======================================================================

	#region Private Fields
	[SerializeField] private Transform _bipedRoot;

	[SerializeField] private Vector3 _moveDirection;
	private Vector3 _screenDirection;
	private Vector3 _jumpDirection;
	private Vector3 _bipedOffsetPos;

	[SerializeField, Range(0.5f, 50f)] private float _moveSpeedMax = 5f;
	[SerializeField, Range(0f, 50f)]   private float _jumpSpeedMax = 10f;
	[SerializeField, Range(0f, 90f)]   private float _jumpDegreesMin;
	[SerializeField, Range(0f, 90f)] private float _frontJumpDegreesMin = 80f;
	[SerializeField, Range(0f, 90f)]   private float _throwDegreesMin;

	[SerializeField] private string _idleTriggerName = "Idle";
	[SerializeField] private string _runTriggerName = "Run";
	[SerializeField] private string _jumpTriggerName = "Jump";
	[SerializeField] private string _landTriggerName = "Land";
	[SerializeField] private string _standUpTriggerName = "StandUp";
	[SerializeField] private string _ticklesTriggerName = "Tickles";

	[SerializeField] private string _ragdollPath = "Prefabs/Ragdoll.prefab";
	[SerializeField] private string _wallLayerName = "Wall";
	
	private Animator _animator;
	private CharacterController _controller;
	private Ragdoll _ragdoll;

	private State _currentState = State.Moving;
	private float _moveRatio;

	private Vector3 _idlePos;
	private Vector3 _jumpVelocity;
	private Vector3 _jumpStartVelocity;
	
	private int _wallLayer;
	private bool _isParaboling;

	private Sense _jumpSense;

	private enum Sense
	{
		Left = -1,
		None = 0,
		Right = 1
	}

	private struct StateActions
	{
		public readonly Action<State> OnStart;
		public readonly Action<State> OnEnd;
		public readonly Action Update;

		public StateActions(Action<State> onStart, Action<State> onEnd, Action update)
		{
			OnStart = onStart;
			OnEnd = onEnd;
			Update = update;
		}
	}

	private Dictionary<State, StateActions> _stateActions;
	

	#endregion
}
