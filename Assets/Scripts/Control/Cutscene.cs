using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cutscene : MonoBehaviour
{
	#region Public Fields
	public bool IsPlaying { get; private set; }
	#endregion


	//===================================================================

	#region Public Methods

	public void Play()
	{
		IsPlaying = true;
		_game.Player.SetIsControllable(false);
		_animator.SetTrigger(_playTriggerName);
	}

	#endregion

	//======================================================================


	#region Mono

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_driver = GetComponentInChildren<CutsceneDriver>();
		_game = MetaManager.Instance.GetManager<GameManager>();
	}

	private void OnDestroy()
	{
		_game = null;
		_animator = null;
		_driver = null;
	}

	private void OnEnable()
	{
		_animator.GetBehaviour<CutsceneEndState>().End += OnCutsceneEnd;
	}

	

	private void OnDisable()
	{
		_animator.GetBehaviour<CutsceneEndState>().End -= OnCutsceneEnd;
	}
	

	private void Update()
	{
		if(_driver.MustFollow)
			_game.Player.GoTo(_driver.transform.position);
	
		_game.Player.CurrentEmotion = _driver.MustShowEmotion ? 
				_emotion : BoomonController.Emotion.None;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(IsPlaying || other.gameObject != _game.Player.gameObject)
			return;

		Play();
	}

	

	#endregion


	//===================================================================

	#region Events

	public void OnEmotionClick(int index)
	{
		Debug.Log("Cutscene::OnEmotionClick>> " + index);
		_animator.SetInteger(_emotionIntName, index);
	}


	public void OnResolutionClick(int index)
	{
		Debug.Log("Cutscene::OnEmotionClick>> " + index);
		_animator.SetInteger(_resolutionIntName, index);
	}

	private void OnCutsceneEnd()
	{
		IsPlaying = false;

		_game.Player.CurrentEmotion = BoomonController.Emotion.Happiness;

		Transition.Instance.AnimEnd += OnTransitionEnd;
		Transition.Instance.StartAnim(2f);	
	}

	private void OnTransitionEnd()
	{
		Transition.Instance.AnimEnd -= OnTransitionEnd;
		SceneLoader.Instance.GoToSelectionMenu();
		Transition.Instance.StartAnim(1f, true);
	}

	#endregion


	//==================================================

	#region Private Fields

	[SerializeField] private BoomonController.Emotion _emotion;
	[SerializeField] private string _playTriggerName = "Play";
	[SerializeField] private string _playerTag = "Player";
	[SerializeField] private string _emotionIntName = "Emotion";
	[SerializeField] private string _resolutionIntName = "Resolution";

	private GameManager _game;
	private Animator _animator;
	private CutsceneDriver _driver;
	

	#endregion

}
