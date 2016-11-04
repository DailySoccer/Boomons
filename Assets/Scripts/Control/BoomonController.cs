using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


// TODO Automatizar configuración (animators y lo que se pueda) a partir de esto
public enum BoomonRole
{
	Artist = 0,
	Gamer = 1,
	Maker = 2,
	Music = 3,
	Naturalist = 4,
	FemaleSport = 5,
	MaleSport = 6
}



[RequireComponent(typeof(Animator), typeof(CharacterController))]
[RequireComponent(typeof(Touchable), typeof(FacialAnimator))]
public class BoomonController : MonoBehaviour, ITeleportable
{
	#region Public Fields
	public enum State
	{
		Idle	= 0,
		Driven	= 1,
		Emotion = 2,
		Move	= 3,
		Throw	= 4,
		StandUp = 5,
		Tickles = 6,
	};

	public enum Emotion
	{
		None = 0,
		Anger = 1,
		Shame = 2,
		Happiness = 3,
		Sadness = 4,
		Scare =	5,
	}

	public enum Sense
	{
		Left = -1,
		None = 0,
		Right = 1
	}

	
	public Vector3 Position {
		get {
			return CurrentState == State.Throw ?
				Ragdoll.Position : transform.position;
		}
	}

	public BoomonRole Role
	{
		get { return _role; }
	}

	
	public State CurrentState
	{
		get { return _currentState; }
		set
		{
			if (value == _currentState)
				return;
			
			State lastState = _currentState;
			_currentState = value;

			_stateActions[lastState].OnEnd(value);
			_stateActions[_currentState].OnStart(lastState);
		}
	}


	public Emotion CurrentEmotion
	{
		get { return _currentEmotion; }
		set 
		{
			if(value == _currentEmotion)
				return;
			_currentEmotion=value;

			CurrentState = value == Emotion.None ? 
				State.Idle : State.Emotion;
			
			if(value != Emotion.None)
				PlayAnimation(value.ToString());
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

	public void SetIsControllable(bool value)
	{
		// TODO FRS 161027 Gestionar el ragdoll
		if(!value && CurrentState == State.Move)
			CurrentState = State.Idle;

		IsTouchEnabled = _isControllable = value;
	}


	public bool IsTouchEnabled
	{
		get{ return _touchable.enabled; }
		private set { _touchable.enabled = value && _isControllable; }
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
		CurrentState = State.Move;
	}

	//public void Jump(Vector2 swipeVector, float speedRatio)
	//{
	//	if (CurrentState == State.Jump || CurrentState == BoomonState.Throw)
	//		return;

	//	float jumpDegress = Mathf.Atan(swipeVector.y / Mathf.Abs(swipeVector.x)) * Mathf.Rad2Deg;
	//	if (!(jumpDegress > _jumpDegreesMin))
	//		return;

	//	MoveSense = jumpDegress > _frontJumpDegreesMin ?
	//		Sense.None : (Sense)Mathf.Sign(swipeVector.x);

	//	_jumpStartVelocity = CalculateJumpSpeed(swipeVector, speedRatio);
	//	CurrentState = State.Jump;
	//}

	public void Throw(Vector2 swipePos, Vector2 swipeVector, float speedRatio)
	{
		if(CurrentState == State.Throw)
			return;

		float throwDegress = Mathf.Atan(swipeVector.y / Mathf.Abs(swipeVector.x)) * Mathf.Rad2Deg;
		if (!(throwDegress > _throwDegreesMin*Mathf.Deg2Rad))
			return;

		CurrentState = State.Throw;
		Ragdoll.Setup(transform, _refSystem);
		Ragdoll.OnSwipe(null, swipePos, swipeVector, speedRatio);
	}

	public void TeleportTo(Teleport target)
	{
		IsTeleporting = true;
		CurrentState = State.Driven;

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
		CurrentState = State.Driven;
	}

	#endregion

	//==============================================================
	
	#region Mono

	// TODO Extraer subinicializaicones
	private void Awake()
	{
		_isControllable = true;

		Debug.Assert(_right != Vector3.zero, "BoomonController::Awake>> Right move direction not defined!!");

		_role =  (BoomonRole) Enum.Parse(typeof (BoomonRole), name.Split('(')[0]);

		_goToCallbacks = new List<Action>();
		_stateActions = new Dictionary<State, StateActions>
		{
			{ State.Driven,  new StateActions(OnDrivenStart, OnDrivenEnd, DrivenUpdate)},
			{ State.Emotion, new StateActions(OnEmotionStart, OnEmotionEnd) },
			{ State.Idle,    new StateActions(OnIdleStart, OnIdleEnd, IdleUpdate)},
			{ State.Move,    new StateActions(OnMoveStart, OnMoveEnd, MoveUpdate, OnMoveCollision)},
			{ State.Throw,   new StateActions(OnThrowStart, OnThrowEnd)},
			{ State.StandUp, new StateActions(OnStandUpStart, OnStandUpEnd)},
			{ State.Tickles, new StateActions(OnTickleStart, OnTickleEnd)},
		};
		

		_refSystem = new ReferenceSystem(transform.position, _right);
		transform.up = _refSystem.JumpDir;

		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();
		_touchable = GetComponent<Touchable>();
		_face = GetComponent<FacialAnimator>();
		_groundSlopeCosine = Mathf.Cos(_controller.slopeLimit * Mathf.Deg2Rad);

		//if (_bipedRoot == null)
		//	_bipedRoot = GetComponentsInChildren<Transform>()
		//		.First(t => t.name.Contains("Bip"));

		//_bipedOffsetPos = _bipedRoot.position - transform.position;
	}



	private void OnDestroy()
	{
		if (_ragdoll != null)
		{
			_ragdoll.GroundEnter -= OnRagdollGroundEnter;
			Destroy(_ragdoll.gameObject);
			_ragdoll = null;
		}

		_goToCallbacks = null;

		_touchable = null;
		_face = null;
		_ragdoll = null;
		//_bipedRoot = null;
		_animator = null;
		_controller = null;
	}
						  
	private void Start()
	{
		MoveSense = Sense.None;
		CurrentState = State.Idle;
	}

	// TODO FRS 161027 Enfocar toda esta clase a un patrón estado inyectándonos en ellos
	private void OnEnable()
	{
		_animator.GetBehaviour<BoomonIdleState>().Start += OnIdleReady;
	}

	private void OnDisable()
	{
		_animator.GetBehaviour<BoomonIdleState>().Start -= OnIdleReady;
	}
	/// <summary>
	/// 
	/// </summary>
	private void Update()
	{
		Action update = _stateActions[CurrentState].Update;
		if (update != null)
			update();

#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.E))
			CurrentEmotion = (Emotion) ((int)(CurrentEmotion + 1) % Enum.GetValues(typeof(Emotion)).Length);
#endif
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

	private void OnRagdollGroundEnter(Vector3 groundPos)
	{
		transform.position = groundPos;
		CurrentState = State.Idle;
	}

	//----------------------------------------------------------------------------------
	
	#region Events.Touch

	public void OnTapStart(GameObject go, Vector2 touchPos)
	{
		if (go == null && (CurrentState == State.Idle || CurrentState == State.Emotion) ) 
			CurrentState = State.Move;
	}

	public void OnTapStop(GameObject go, Vector2 position)
	{
		if (CurrentState == State.Move)
			CurrentState = State.Idle;
	}

	public void OnTapStay(GameObject go, Vector2 touchPos)
	{
		if (go == null && CurrentState == State.Move)
			Move(touchPos);
	}

	public void OnDoubleTap(GameObject go, Vector2 position)
	{
		if ((CurrentState==State.Idle||CurrentState==State.Emotion) && go == gameObject)
			CurrentState = State.Tickles;
	}

	public void OnSwipe(GameObject go, Vector2 position, Vector2 direction, float speedRatio)
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

	#endregion

	//---------------------------------------------------------
	
	#region Events.Idle

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
		transform.position = _refSystem.ProjectOnPlane(transform.position);

		switch (lastState)
		{
			case State.Throw:
			case State.StandUp:
				PlayAnimation(_standUpTriggerName);
				break;
			case State.Tickles:
				PlayAnimation(lastState.ToString());
				break;
			//case State.Jump:
			//	PlayAnimation(_landTriggerName);
			//	break;
			default:
				PlayAnimation(State.Idle.ToString());
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

		IsTouchEnabled = nextState == State.Tickles
						 || nextState == State.Move
						 || nextState == State.Emotion;
	}
	
	private void IdleUpdate()
	{
		_controller.SimpleMove(Vector3.zero);
	}

	#endregion

	//----------------------------------------------------------------------

	#region Events.Move

	private void OnMoveStart(State lastState)
	{
		Log("OnMoveStart");
		PlayAnimation(State.Move.ToString());
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

		if (rigid != null)
		{
			Push(rigid);
			transform.position = _refSystem.ProjectOnPlane(transform.position);
		}
		else if (Vector3.Dot(hit.normal, _refSystem.JumpDir) < _groundSlopeCosine)
		{
			CurrentState = State.Idle;
		}
	}

	#endregion

	//---------------------------------------------------------------------------------

	#region Events.Throw

	private void OnThrowStart(State lastState)
	{
		Log("OnThrowStart");
		gameObject.SetActive(false);
		Ragdoll.gameObject.SetActive(true);
	}

	private void OnThrowEnd(State nextState)
	{
		Log("OnThrowEnd");

		_face.Reset();
		Ragdoll.gameObject.SetActive(false);
		gameObject.SetActive(true);
	}

	#endregion

	//----------------------------------------------------

	#region Events.Jump


	private void OnStandUpStart(State obj)
	{
		CurrentState = State.Idle;
	}

	private void OnStandUpEnd(State obj)
	{

	}

	#endregion

	//----------------------------------------------------

	#region Events.Jump

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

	#endregion

	//------------------------------------------------------------------

	#region Events.Tickle

	private void OnTickleStart(State obj)
	{
		Log("OnTickleStart");
		CurrentState = State.Idle;
	}

	private void OnTickleEnd(State obj)
	{
		Log("OnTickleEnd");
	}

	#endregion

	//-------------------------------------------------------------------

	#region Events.CodeDrive

	private void OnDrivenStart(State obj)
	{
		Log("OnDrivenStart");
		PlayAnimation(State.Move.ToString());
	}

	private void OnDrivenEnd(State obj)
	{
		Log("OnDrivenEnd");

		IsTeleporting = false;
	}
	
	private void DrivenUpdate()
	{
		bool hasReachedTarget = Vector3.Dot(_velocity, transform.position - _drivenTarget) >= 0f;
		if (hasReachedTarget)
			OnDrivenTargetReached(); 
		else
			_controller.SimpleMove(_velocity);
	}

	private void OnDrivenTargetReached()
	{
		CurrentState = State.Idle;

		int n = _goToCallbacks.Count;
		for(int i = 0; i < n; ++i)
			_goToCallbacks[i]();

		_goToCallbacks.RemoveRange(0, n);
	}

	#endregion

	//------------------------------------------------------------------

	#region Events.Emotion

	private void OnEmotionStart(State obj)
	{
	}

	private void OnEmotionEnd(State obj)
	{
		_currentEmotion = Emotion.None;
	}

	#endregion

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

	private Ragdoll Ragdoll
	{
		get
		{
			if (_ragdoll == null) {
				string ragdollPath = PathSolver.Instance.GetBoomonPath(_role, PathSolver.InstanceType.Ragdoll);
				var ragdollPrefab = Resources.Load<GameObject>(ragdollPath);
				_ragdoll = Instantiate(ragdollPrefab).GetComponent<BoomonRagdoll>();
				_ragdoll.GroundEnter += OnRagdollGroundEnter;
			}
			return _ragdoll;
		}
	}

	


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

	//[SerializeField] private string _jumpTriggerName = "Jump";
	//[SerializeField] private string _landTriggerName = "Land";
	[SerializeField] private string _standUpTriggerName = "StandUp";
	

	

	private Animator _animator;
	private CharacterController _controller;
	private BoomonRagdoll _ragdoll;
	private FacialAnimator _face;
	private Touchable _touchable;

	private Vector3 _velocity;
	private Vector3 _jumpStartVelocity;
	
	private bool _isParaboling;
	private float _groundSlopeCosine;

	private Sense _moveSense;
	private State _currentState = State.Move;

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


	private ReferenceSystem _refSystem;
	private Dictionary<State, StateActions> _stateActions;
	private List<Action> _goToCallbacks;
	private Vector3 _drivenTarget;
	private BoomonRole _role;
	private Emotion _currentEmotion;

	[SerializeField] private BoomonIdleState _idleStateState;
	private bool _isControllable;

	#endregion


}
