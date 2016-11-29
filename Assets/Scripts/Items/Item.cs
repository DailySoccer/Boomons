using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Item : MonoBehaviour
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

	protected virtual void Awake()
	{
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

	protected virtual void OnDestroy()
	{
		_audio = null;
		_animator = null;
		_toucher = null;
		_game = null;
	}

	protected virtual void OnEnable()
    {
        if (_playOnEnable)
            Play();

		StartCoroutine(InteractableChecker());
	}

	protected virtual void OnDisable()
	{
		StopAllCoroutines();
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


	private IEnumerator InteractableChecker()
	{
		for(;;) {
			IsInteractable = IsBoomonWithinInteractionRadius();
			yield return InteractableCheckYield;
		}
	}


	#endregion

	//======================================================================

	#region Private Methods

	private bool IsBoomonWithinInteractionRadius()
	{
		if (_game.Boomon == null)
			return false;

		Vector3 boomonDist = _game.Boomon.transform.position - transform.position;
		boomonDist = _game.Boomon.ReferenceSystem.ProjectOnPlane(boomonDist, false);
		float boomonDistSqr = Vector3.SqrMagnitude(boomonDist);

		return boomonDistSqr < _interactableRadio * _interactableRadio;
	}

	#endregion

	//======================================================================

	#region Events

	public void OnTapStart(Toucher toucher, Vector2 touchPos)
	{

		if(_game.Boomon.CurrentState == BoomonController.State.Idle)
			Play();
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
	[SerializeField, Range(0f, 10f)] private float _interactableRadio = 4f;
	[SerializeField] private string _playTriggerName = "Play";

	public readonly WaitForSeconds InteractableCheckYield = new WaitForSeconds(.25f);

	private static GameManager _game;
	private Animator _animator;
	private AudioSource _audio;

	private bool _isInteractable;
	private Toucher _toucher;

	#endregion
}
