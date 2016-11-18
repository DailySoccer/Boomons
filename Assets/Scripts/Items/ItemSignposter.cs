using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class ItemSignposter : MonoBehaviour
{

	#region Public Fields

	public bool IsActive
	{
		get { return _isActive; }
		private set
		{
			IsVisible = value;
			_itemToSignpost.IsTouchEnabled = value;

			if(value == _isActive)
				return;
			_isActive = value;

			if (value)
				OnActivate();
			else
				OnDeactivate();
		}
	}

	public bool IsVisible
	{
		get { return _isVisible;  }
		private set
		{
			_isVisible = value;
			foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
				r.enabled = value;
		}
	}

	#endregion

	//==============================================================================


	#region MONO

	private void Awake()
	{
		_animator = GetComponentInChildren<Animator>();
		_audio = GetComponent<AudioSource>();

		if(_itemToSignpost == null) {
			Debug.LogWarning("ItemSignposter::Start>> No target defined; deactivating...", this);
			enabled = false;
		}
	}

	private void Start()
	{
		_boomon = MetaManager.Instance.Get<GameManager>().Player;
		IsActive = false;
	}
	
	private void OnDestroy()
	{
		_audio = null;
		_itemToSignpost = null;
		_boomon = null;
		_animator = null;
		_activationClip = null;
		_deactivationClip = null;
	}

	// TODO FRS 161117 Se puede bajar la frecuencia de esta detección
	// TODO FRS 161117 Si usamos corrutinas podríamos recurrir al enabled en lugar de IsVisible?
	private void Update()
	{
		Vector3 boomonDist = _boomon.transform.position - _itemToSignpost.transform.position;
		boomonDist = _boomon.ReferenceSystem.ProjectOnPlane(boomonDist, false);
		float boomonDistSqr = Vector3.SqrMagnitude(boomonDist);

		IsActive = boomonDistSqr < _activationDistance*_activationDistance;
	}


	#endregion

	//===================================================================================


	#region Events

	private void OnActivate()
	{
		Debug.Log("ItemSignposter::Activate>> " + _itemToSignpost.name, this);
		_animator.SetTrigger("Play");
		_audio.PlayOneShot(_activationClip);
	}

	private void OnDeactivate()
	{
		_audio.PlayOneShot(_deactivationClip);
	}

	#endregion

	//=======================================================================================

	#region Private Fields

	[SerializeField] private Animator _animator;
	[SerializeField] private ItemActivator _itemToSignpost;
	[SerializeField, Range(0f, 10f)] private float _activationDistance = 2f;
	[SerializeField] private AudioClip _activationClip;
	[SerializeField] private AudioClip _deactivationClip;

	private AudioSource _audio;
	private BoomonController _boomon;
	private bool _isActive;
	private bool _isVisible;

	#endregion
}
