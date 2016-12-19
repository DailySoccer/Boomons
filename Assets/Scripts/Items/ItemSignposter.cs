using System;
using System.Collections;
using System.Runtime.Remoting;
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

	public void Show()
	{
		Debug.Log("ItemSignposter::Show>> " + name, this);

		gameObject.SetActive(true);

		if(_animator != null)
			_animator.SetTrigger(_activation.Trigger);

		PlayClip(_activation.Clip);
	}

	public void Hide()
	{
		Debug.Log("ItemSignposter::Hide>> " + name, this);

		if(_animator != null)
			_animator.SetTrigger(_deactivation.Trigger);

		PlayClip(_deactivation.Clip);
		DelayedDeactivate(_animator.GetCurrentAnimatorStateInfo(0).length);
	}

	
	#endregion

	//==============================================================================


	#region MONO

	private void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_audio = GetComponent<AudioSource>();	

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
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator DelayedDeactivateCoroutine(float secs)
	{
		yield return new WaitForSeconds(secs);
		gameObject.SetActive(false);
	}

	#endregion

	//===================================================================================


	#region Events

	private void OnItemInteractableChange(bool value)
	{
		StopAllCoroutines();

		if (value)
			Show();
		else
			Hide();
	}



	#endregion

	//=======================================================================

	#region Private Methods

	private void PlayClip(AudioClip clip)
	{
		if(_audio == null)	
			_audio = gameObject.AddComponent<AudioSource>();

		_audio.PlayOneShot(clip);
	}

	private void DelayedDeactivate(float secs)
	{
		if(enabled)
			StartCoroutine(DelayedDeactivateCoroutine(secs));
	}

	#endregion

	//=======================================================================================

	#region Private Fields

	private AudioSource _audio;
	private Animator _animator;

	[SerializeField] private Item _itemToSignpost;

	[SerializeField] private AnimationData _activation 
		= new AnimationData() { Trigger = "Play", Clip = null};
	[SerializeField] private AnimationData _deactivation;

	

	#endregion
}
