using System;
using UnityEngine;

[ExecuteInEditMode]
public class CutsceneDriver : MonoBehaviour
{
	#region Public Fields

	public event Action<bool> BoomonActiveChange;
	public event Action<BoomonController.State>   BoomonStateChange;
	public event Action<BoomonController.Emotion> BoomonEmotionChange;

	public bool IsBoomonActive
	{
		get { return _isBoomonActive; }
		private set
		{
			if (value == _isBoomonActive)
				return;
			_isBoomonActive = _animableBoomonActive = value;
			OnBoomonActiveChange(value);
		}
	}

	

	public BoomonController.State BoomonState 
	{
		get { return _boomonState; }
		private set
		{
			if (value == _boomonState)
				return;

			_boomonState = value;
			_animableBoomonState = (float)value;

			OnBoomonStateChange(value);

			if (value != BoomonController.State.Emotion) {
				_boomonEmotion = BoomonController.Emotion.None;
				_animableBoomonEmotion = (float) BoomonController.Emotion.None;
			}
		}
	}

	

	public BoomonController.Emotion BoomonEmotion  
	{
		get { return _boomonEmotion; }
		private set
		{
			if(value == _boomonEmotion)
				return;
			_boomonEmotion = value;
			_animableBoomonEmotion = (float) value;

			BoomonState = value == BoomonController.Emotion.None ?
				BoomonController.State.Idle : BoomonController.State.Emotion;

			OnBoomonEmotionChange(value);
		}
	}


	//public Vector3 BoomonRight { get { return _boomonRight;  } }

	#endregion

	//==========================================================================================

	#region MONO

	private void OnDestroy()
	{
		BoomonActiveChange = null;
		BoomonStateChange = null;
		BoomonEmotionChange = null;
	}

	private void Update()
	{
		IsBoomonActive = _animableBoomonActive;
		BoomonState = (BoomonController.State)_animableBoomonState;
		BoomonEmotion = (BoomonController.Emotion)_animableBoomonEmotion;
	}

	#endregion

	//======================================================================

	#region Private Methods

	private void OnBoomonActiveChange(bool value)
	{
		var e = BoomonActiveChange;
		if(e != null)
			e(value);
	}

	private void OnBoomonStateChange(BoomonController.State value)
	{
		var e = BoomonStateChange;
		if (e != null)
			e(value);
	}

	private void OnBoomonEmotionChange(BoomonController.Emotion value)
	{
		var e = BoomonEmotionChange;
		if (e != null)
			e(value);
	}

	#endregion


	//===========================================================================================

	#region Private Fields

	//[SerializeField] private Vector3 _boomonRight;
	[SerializeField] private bool _animableBoomonActive = true;
	[SerializeField] private float _animableBoomonState;
	[SerializeField] private float _animableBoomonEmotion;


	private bool _isBoomonActive;
	private BoomonController.State _boomonState;
	private BoomonController.Emotion _boomonEmotion;
	

	#endregion

}
