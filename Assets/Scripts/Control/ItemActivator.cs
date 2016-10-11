using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class ItemActivator : Touchable
{

	#region Public Methods

	public void Activate()
	{
		Debug.Log("ItemActivator::Activate");
		
		_animator.SetTrigger(_playTriggerName);
		_audio.Play();
	}

	#endregion

	//======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();
		_animator = GetComponent<Animator>();
		_audio = GetComponent<AudioSource>();
	}

	protected override void OnDestroy()
	{
		_audio = null;
		_animator = null;
		base.OnDestroy();
	}

	private void OnCollisionEnter(Collision info)
	{
		if (info.gameObject.tag == _playerTag)
			Activate();
	}

	#endregion

	//======================================================================

	#region Events

	public override void OnTapStart(GameObject go, Vector2 touchPos)
	{
		Activate();
	}

	
	#endregion

	//====================================================

	#region Private Fields

	private Animator _animator;
	private AudioSource _audio;
	[SerializeField] private string _playTriggerName = "Play";
	[SerializeField] private string _playerTag = "Player";

	#endregion
}
