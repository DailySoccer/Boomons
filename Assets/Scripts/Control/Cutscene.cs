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
		_game = MetaManager.Instance.Get<GameManager>();
	}

	private void OnDestroy()
	{
		_game = null;
		_animator = null;
		_driver = null;
	}

	private void OnEnable()
	{
		var endState = _animator.GetBehaviour<CutsceneEndState>();
		if(endState != null)
			endState.End += OnCutsceneEnd;

		_driver.BoomonActiveChange	+= OnBoomonActiveChange;
		_driver.BoomonStateChange	+= OnBoomonStateChange;
		_driver.BoomonEmotionChange += OnBoomonEmotionChange;
	}

	private void OnDisable()
	{
		StopAllCoroutines();

		var endState = _animator.GetBehaviour<CutsceneEndState>();
		if(endState != null)
			endState.End -= OnCutsceneEnd;

		_driver.BoomonStateChange	-= OnBoomonStateChange;
		_driver.BoomonEmotionChange -= OnBoomonEmotionChange;
		_driver.BoomonActiveChange  -= OnBoomonActiveChange;
	}



	private void OnTriggerEnter(Collider other)
	{
		if(IsPlaying || other.gameObject != _game.Boomon.gameObject)
			return;

		Play();
	}



	#endregion


	//===================================================================

	#region Events


	private void OnBoomonActiveChange(bool isActive)
	{
		_game.Boomon.gameObject.SetActive(isActive);
	}


	private void OnBoomonStateChange(BoomonController.State state)
	{
		if (state == BoomonController.State.Driven) 
			_game.Boomon.GoTo(_driver.transform.position, _driver.BoomonMoveSpeed);
		else
			_game.Boomon.CurrentState = state;
	}
		
	private void OnBoomonEmotionChange(BoomonController.Emotion emotion)
	{
		_game.Boomon.CurrentEmotion = emotion;
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

		_game.Boomon.CurrentEmotion = BoomonController.Emotion.Happiness;

		Transition.Instance.AnimEnd += OnTransitionEnd;
		Transition.Instance.StartAnim(2f);	
	}

	private void OnTransitionEnd()
	{
		Transition.Instance.AnimEnd -= OnTransitionEnd;
		SceneLoader.Instance.GoToMainMenu();
		Transition.Instance.StartAnim(1f, true);
	}

	private IEnumerator PlayCoroutine()
	{
		IsPlaying = true;
		_game.Boomon.SetIsControllable(false);

		// TODO FRS 161104 Event StateChange
		yield return new WaitUntil(
			() => _game.Boomon.CurrentState == BoomonController.State.Idle);

		_animator.SetTrigger(_playTriggerName);
	}

	#endregion


	//==================================================

	#region Private Fields

	[SerializeField] private string _playTriggerName = "Play";
	[SerializeField] private string _emotionIntName = "Emotion";
	[SerializeField] private string _resolutionIntName = "Resolution";

	private GameManager _game;
	private Animator _animator;
	private CutsceneDriver _driver;
	

	#endregion

}
