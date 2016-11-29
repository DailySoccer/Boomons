using System;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class ItemSignposter : MonoBehaviour
{

	#region Public Fields

	[Serializable]
	public struct AnimationData
	{
		public string Trigger;
		public AudioClip Clip;
	}

	//public void Show()
	//{
	//	gameObject.SetActive(true);

	//	_animator.SetTrigger("Play");
	//	_audio.PlayOneShot(_activationClip);
	//}

	//public void Hide()
	//{	
	//	_audio.PlayOneShot(_deactivationClip);
	//	gameObject.SetActive(false);
	//}

	#endregion

	//==============================================================================


	#region MONO

	private void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_audio = GetComponent<AudioSource>();
		if (_audio == null)
			_audio = gameObject.AddComponent<AudioSource>();

		if (_itemToSignpost == null) {
			Debug.LogWarning("ItemSignposter::Awake>> No target defined; deactivating...", this);
			enabled = false;

		} else {
			_itemToSignpost.InteractableChange += OnItemInteractableChange;
		}

		gameObject.SetActive(false);
	}				
	
	private void OnDestroy()
	{
		if(_itemToSignpost != null)
			_itemToSignpost.InteractableChange -= OnItemInteractableChange;
		
		_audio = null;
		_itemToSignpost = null;	  
		_animator = null;
	}

	private void OnEnable()
	{
		if(_animator != null)
			_animator.SetTrigger(_activation.Trigger);

		if(_activation.Clip != null)
			_audio.PlayOneShot(_activation.Clip);
		else
			_audio.Play();
	}

	private void OnDisable()
	{
		if(_animator != null)
			_animator.SetTrigger(_deactivation.Trigger);

		_audio.PlayOneShot(_deactivation.Clip);
	}

	#endregion

	//===================================================================================


	#region Events

	private void OnItemInteractableChange(bool value)
	{
		gameObject.SetActive(value);
	}



	#endregion

	//=======================================================================================

	#region Private Fields

	private AudioSource _audio;

	[SerializeField] private Animator _animator;
	[SerializeField] private Item _itemToSignpost;

	[SerializeField] private AnimationData _activation 
		= new AnimationData() { Trigger = "Play", Clip = null};
	[SerializeField] private AnimationData _deactivation;

	

	#endregion
}
