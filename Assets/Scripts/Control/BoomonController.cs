using System;
using System.Collections.Generic;
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
		Throwing = 2
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

			switch (lastState)
			{
				case State.Moving:
					OnMoveEnd();
					break;
				case State.Throwing:
					OnThrowEnd();
					break;
			}

			switch (value)
			{
				case State.Moving:
					OnMoveStart();
					break;
				case State.Throwing:
					OnThrowStart();
					break;
			}
		}
	}

	#endregion


	//==============================================================


	#region Mono

	protected override void Awake()
	{
		base.Awake();

		Debug.Assert(_moveDirection != Vector3.zero, "BoomonController::Awake>> Move direction not defined!!");

		_wallLayer = LayerMask.NameToLayer(_wallLayerName);
		
		_moveDirection.Normalize();
		_screenDirection = Vector3.Cross(transform.up, _moveDirection).normalized;
		transform.up = Vector3.Cross(_moveDirection, _screenDirection);

		_startPos = transform.position;

		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();

		var ragdollPrefab = Resources.Load<GameObject>(_ragdollPath);
		_ragdoll = Instantiate(ragdollPrefab).GetComponent<Ragdoll>();
		_ragdoll.GroundEnter += OnRagdollGroundEnter;
	}

	
	protected override void OnDestroy()
	{
		_ragdoll.GroundEnter -= OnRagdollGroundEnter;
		_ragdoll = null;

		
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
		_controller.SimpleMove(_speedRatio * _moveSpeedMax * transform.forward);
	}


	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.layer == _wallLayer)
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
		if(CurrentState == State.Throwing)
			return;

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
		_speedRatio = Math.Abs(moveValue);
	}

	public override void OnDoubleTap(GameObject go, Vector2 position)
	{
		_animator.SetTrigger(_ticklesTriggerName);
	}

	public override void OnSwipe(GameObject go, Vector2 swipeVector, float speedRatio)
	{
		if(go != gameObject || CurrentState == State.Throwing || swipeVector.y < 0f)
			return;

		CurrentState = State.Throwing;
		_ragdoll.Setup(transform);
		_ragdoll.OnSwipe(go, swipeVector, speedRatio);
	}

	//---------------------------------------------------------------------------------

	private void OnThrowStart()
	{
		gameObject.SetActive(false);
	}

	private void OnThrowEnd()
	{
		gameObject.SetActive(true);
		OnMoveEnd();
	}

	

	//----------------------------------------------------------------------

	private void OnMoveStart()
	{
		Debug.Log("BoomonController::OnMoveStart");
		
		_animator.SetTrigger(_runTriggerName);
	}

	private void OnMoveEnd()
	{
		Debug.Log("BoomonController::OnMoveEnd");

		_speedRatio = 0f;
		_animator.SetTrigger(_idleTriggerName);

		transform.forward =  _screenDirection;
		transform.position -= Vector3.Project(transform.position - _startPos, _screenDirection);
	}


	//---------------------------------------------------------

	private void OnRagdollGroundEnter(Vector3 groundPos)
	{
		transform.position = groundPos;
		CurrentState = State.Idle;
	}

	#endregion



	//=======================================================================

	#region Private Fields



	[SerializeField] private Vector3 _moveDirection;
	[SerializeField, Range(0.5f, 50f)] private float _moveSpeedMax = 5f;
	
	[SerializeField] private string _idleTriggerName = "Idle";
	[SerializeField] private string _runTriggerName = "Run";
	[SerializeField] private string _ticklesTriggerName = "Tickles";

	[SerializeField] private string _ragdollPath = "Prefabs/Ragdoll.prefab";
	[SerializeField] private string _wallLayerName = "Wall";

	private Animator _animator;
	private CharacterController _controller;
	private Ragdoll _ragdoll;
	

	private State _currentState = State.Moving;
	private Vector3 _startPos;
	private float _speedRatio;
	private Vector3 _screenDirection;
	private int _wallLayer;
	

	#endregion
}
