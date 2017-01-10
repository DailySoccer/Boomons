using System;
using UnityEngine;
using Extension;


[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Item : BoomonProximityDetector
{

	#region Public Fields

	public event Action<bool> InteractableChange;
	 
	public bool IsInteractable 
	{
		get { return _isInteractable; }
		private set
		{
			if(Toucher != null)
				Toucher.enabled = value;

			if(value != _isInteractable) { 
				_isInteractable = value;
				OnInteractableChange(value);
			}
		}
	}

	

	#endregion

	//=========================================================

	#region Public Methods

	public virtual void Play()
	{
		if (!_canPlay)
			return;

		Debug.Log("Item::Play>> " + name, this);

		AudioSource.Play(true);
		if(!string.IsNullOrEmpty(_playTriggerName))
			Animator.SetTrigger(_playTriggerName);

		_canPlay = _canPlayWhilePlaying;
	}

	
	#endregion

	//======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();

		if(Animator == null)
			Animator = GetComponent<Animator>();
		if(AudioSource == null)
			AudioSource = GetComponent<AudioSource>();
		if(Toucher == null)
			Toucher = GetComponent<Toucher>();

		if(_game == null)
			_game = MetaManager.Instance.Get<GameManager>();
	}

	protected override void OnDestroy()
	{
		AudioSource = null;
		Animator = null;
		Toucher = null;
		_game = null;
		base.OnDestroy();
	}

	protected override void OnEnable()
	{
		var idleState = Animator.GetBehaviour<ItemIdleState>();
		if (idleState != null)
			idleState.Enter += OnAnimationIdleEnter;
		else
			_canPlayWhilePlaying = true;

		base.OnEnable();
        if (_playOnEnable)
            Play();
    }


	protected virtual void OnDisable()
	{
		ProximityTarget = null;

		var idleState = Animator.GetBehaviour<ItemIdleState>();
		if(idleState != null)
			idleState.Enter -= OnAnimationIdleEnter;
	}
	  
	protected override void Start()
	{
		base.Start();
		IsInteractable = false;
	}


	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (_isPhysicallyPlayable && collision.gameObject.tag == _game.Boomon.tag)
			Play();
	}

	protected virtual void OnTriggerEnter(Collider other)
    {
        if (_isPhysicallyPlayable && other.gameObject.tag == _game.Boomon.tag)
            Play();
    }

	#endregion



	//======================================================================

	#region Events

	private void OnAnimationIdleEnter()
	{
		_canPlay = true;
	}

	public void OnTapStart(Toucher toucher, Vector2 touchPos)
	{	  
		if(_game.Boomon.CurrentState == BoomonController.State.Idle)
			Play();
	}	 

	protected override void OnTargetEnter()
	{
		base.OnTargetEnter();
		IsInteractable = true;
	}

	protected override void OnTargetExit()
	{
		IsInteractable = false;
		base.OnTargetExit();
	}

	protected virtual void OnInteractableChange(bool value)
	{
		if(value && _playOnInteractable)
			Play();

		var e = InteractableChange;
		if (e != null)
			e(value);
	}

	#endregion


	//====================================================

	#region Private Methods



	#endregion
	//====================================================

	#region Private Fields

	[SerializeField] private bool _canPlayWhilePlaying = false;
	[SerializeField] private bool _isPhysicallyPlayable = false;
	[SerializeField] private bool _playOnEnable = false;
	[SerializeField] private bool _playOnInteractable = false;
	[SerializeField] private string _playTriggerName = "Play";


	private bool _isInteractable;
	private bool _canPlay = true;

	private static GameManager _game;
	protected AudioSource AudioSource { get; private set; }
	protected Animator Animator { get; private set; }
	protected Toucher Toucher { get; private set; }

	#endregion
}
