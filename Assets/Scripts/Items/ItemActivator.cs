using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class ItemActivator : Touchable
{

	#region Public Methods

    [SerializeField]
	public void Activate()
	{
		Debug.Log("ItemActivator::Activate>> " + name, this);
		
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

		if(_game == null)
			_game = MetaManager.Instance.Get<GameManager>();
	}

	protected override void OnDestroy()
	{
		_audio = null;
		_animator = null;
		base.OnDestroy();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_activateOnEnable)
            Activate();
    }


    private void OnCollisionEnter(Collision collision)
	{
		if (_isPhysicallyActivable && collision.gameObject.tag == _playerTag)
			Activate();
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (_isPhysicallyActivable && collider.gameObject.tag == _playerTag)
            Activate();
    }


    #endregion

    //======================================================================

    #region Events

    public override void OnTapStart(GameObject go, Vector2 touchPos)
	{		   
		if(_game.Player.CurrentState == BoomonController.State.Idle)
			Activate();
	}

	
	#endregion

	//====================================================

	#region Private Fields

	private Animator _animator;
	private AudioSource _audio;
	
	[SerializeField] private string _playTriggerName = "Play";
	[SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _activateOnEnable = false;
	[SerializeField] private bool _isPhysicallyActivable = true;
	private static GameManager _game;

	#endregion
}
