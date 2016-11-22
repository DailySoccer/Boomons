using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class ItemSignposter : MonoBehaviour
{

	#region Public Fields

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
		_activationClip = null;
		_deactivationClip = null;
	}

	private void OnEnable()
	{
		_animator.SetTrigger("Play");
		_audio.PlayOneShot(_activationClip);
	}

	private void OnDisable()
	{
		_audio.PlayOneShot(_deactivationClip);
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

	[SerializeField] private Animator _animator;
	[SerializeField] private Item _itemToSignpost;
	[SerializeField] private AudioClip _activationClip;
	[SerializeField] private AudioClip _deactivationClip;

	private AudioSource _audio;

	#endregion
}
