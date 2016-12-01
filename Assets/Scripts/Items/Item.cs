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
		
		_animator.SetTrigger(_playTriggerName);
		_audio.Play();
	}

	#endregion

	//======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();

		if(_animator == null)
			_animator = GetComponent<Animator>();
		if(_audio == null)
			_audio = GetComponent<AudioSource>();
		if(_toucher == null)
			_toucher = GetComponent<Toucher>();

		if(_toucher != null)
			_toucher.MustReceiveOnlyItsTouches = true;

		if(_game == null)
			_game = MetaManager.Instance.Get<GameManager>();
	}

	protected override void OnDestroy()
	{
		_audio = null;
		_animator = null;
		_toucher = null;
		_game = null;
		base.OnDestroy();
	}

	protected override void OnEnable()
    {
		base.OnEnable();
        if (_playOnEnable)
            Play();
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

	private void OnInteractableChange(bool value)
	{
		if(value && _playOnInteractable)
			Play();
			 
		if(_toucher != null)
			_toucher.IsTouchEnabled = value;

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


	private static GameManager _game;
	private Animator _animator;
	private AudioSource _audio;

	private bool _isInteractable;
	private Toucher _toucher;

	#endregion
}
