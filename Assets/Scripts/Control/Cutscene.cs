using System.Collections;
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
		StartCoroutine(PlayCoroutine());
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
		_driver.BoomonActiveChange	+= OnBoomonActiveChange;
		_driver.BoomonStateChange	+= OnBoomonStateChange;
		_driver.BoomonEmotionChange += OnBoomonEmotionChange;
	}

	private void OnDisable()
	{
		StopAllCoroutines();

		_animator.GetBehaviour<CutsceneEndState>().End -= OnCutsceneEnd;
		_driver.BoomonStateChange	-= OnBoomonStateChange;
		_driver.BoomonEmotionChange -= OnBoomonEmotionChange;
		_driver.BoomonActiveChange  -= OnBoomonActiveChange;
	}



	private void OnTriggerEnter(Collider other)
	{
		if(IsPlaying || other.tag != _playerTag)
			return;

		Play();
	}



	#endregion


	//===================================================================

	#region Events


	private void OnBoomonActiveChange(bool isActive)
	{
		_game.Player.gameObject.SetActive(isActive);
	}


	private void OnBoomonStateChange(BoomonController.State state)
	{
		if (state == BoomonController.State.Driven) 
			_game.Player.GoTo(_driver.transform.position);
		else
			_game.Player.CurrentState = state;
	}
		
	private void OnBoomonEmotionChange(BoomonController.Emotion emotion)
	{
		_game.Player.CurrentEmotion = emotion;
	}
	
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

	private IEnumerator PlayCoroutine()
	{
		IsPlaying = true;
		_game.Player.SetIsControllable(false);

		// TODO FRS 161104 Event StateChange
		yield return new WaitUntil(
			() => _game.Player.CurrentState == BoomonController.State.Idle);

		_animator.SetTrigger(_playTriggerName);
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
