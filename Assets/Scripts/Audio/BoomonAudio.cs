using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoomonController))]
public class BoomonAudio : MonoBehaviour
{

	#region Public Fields

	[Serializable]
	public class StateAudioClip
	{
		public BoomonController.State State;
		public AudioClip File;
	}

	[Serializable]
	public class EmotionAudioClip
	{
		public BoomonController.Emotion Emotion;
		public AudioClip File;
	}

	#endregion

	//=================================================================

	#region Mono

	private void Awake()
	{
		_controller = GetComponent<BoomonController>();
		Debug.Assert(_controller != null, "BoomonAudio::Awake>> Controller not defined!!", this);

		_source = GetComponent<AudioSource>();
		if (_source == null)
			_source = gameObject.AddComponent<AudioSource>();
	}

	private void OnDestroy()
	{
		_controller = null;
		_stateClips = null;
		_emotionClips = null;
		_source = null;
		_leftStep = null;
		_rightStep = null;
	}

	private void OnEnable()
	{
		_controller.StateChange += OnStateChange;
		_controller.EmotionChange += OnEmotionChange;
	}

	private void OnDisable()
	{
		_controller.StateChange -= OnStateChange;
		_controller.EmotionChange -= OnEmotionChange;
	}

	
	#endregion

	//=================================================================

	#region Events

	// TODO FRS 161213 Array de animation event clips
	private void OnAnimationEvent(string eventName)
	{
		switch (eventName)
		{
			case "LeftStep":
				_source.PlayOneShot(_leftStep);
				break;
			case "RightStep":
				_source.PlayOneShot(_rightStep);
				break;
			default:
				break;
		}
	}

	private void OnStateChange(
		BoomonController.State lastState, 
		BoomonController.State nextState)
	{
		_source.Stop();

		StateAudioClip clip = _stateClips.FirstOrDefault(a => a.State == nextState);
		if (clip != null)
			_source.PlayOneShot(clip.File);
	}



	private void OnEmotionChange(
		BoomonController.Emotion lastEmotion,
		BoomonController.Emotion nextEmotion)
	{
		_source.Stop();

		EmotionAudioClip clip = _emotionClips.FirstOrDefault(a => a.Emotion == nextEmotion);
		if(clip != null)
			_source.PlayOneShot(clip.File);
	}

	#endregion

	

	//============================================================

	#region Private Fields


	[SerializeField] private AudioClip _leftStep;
	[SerializeField] private AudioClip _rightStep;
	[SerializeField] private StateAudioClip[] _stateClips;
	[SerializeField] private EmotionAudioClip[] _emotionClips;

	private AudioSource _source;
	private BoomonController _controller;

	#endregion
}
