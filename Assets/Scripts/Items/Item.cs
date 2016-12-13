using System;
using UnityEngine;

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
			if (value == _isInteractable)
				return;

			_isInteractable = value;
			OnInteractableChange(value);
		}
	}

	

	#endregion

	//=========================================================

	#region Public Methods

	public virtual void Play()
	{
		Debug.Log("Item::Play>> " + name, this);

		Audio.Play();
		if(!string.IsNullOrEmpty(_playTriggerName))
			Animator.SetTrigger(_playTriggerName);
	}

	#endregion

	//======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();

		if(Animator == null)
			Animator = GetComponent<Animator>();
		if(Audio == null)
			Audio = GetComponent<AudioSource>();
		if(Toucher == null)
			Toucher = GetComponent<Toucher>();

		if(_game == null)
			_game = MetaManager.Instance.Get<GameManager>();
	}

	protected override void OnDestroy()
	{
		Audio = null;
		Animator = null;
		Toucher = null;
		_game = null;
		base.OnDestroy();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
        if (_playOnEnable)
            Play();
    }
	  
	protected virtual void OnDisable()
	{
		ProximityTarget = null;
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
			 
		if(Toucher != null)
			Toucher.enabled = value;

		var e = InteractableChange;
		if (e != null)
			e(value);
	}

	#endregion

	//====================================================

	#region Private Fields

    [SerializeField] private bool _playOnEnable = false;
	[SerializeField] private bool _playOnInteractable = false;
	[SerializeField] private bool _isPhysicallyPlayable = false;
	
	[SerializeField] private string _playTriggerName = "Play";


	private bool _isInteractable;

	private static GameManager _game;
	protected AudioSource Audio { get; private set; }
	protected Animator Animator { get; private set; }
	protected Toucher Toucher { get; private set; }

	#endregion
}
