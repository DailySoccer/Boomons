using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : Singleton<LoadScreen>
{
	#region Public Fields

	public float ProgressRatio {
		get { return _slider.value; }
		set { _slider.value = Mathf.Clamp01(value); }
	}

	public bool IsVisible
	{
		get { return gameObject.activeSelf; }
		set { gameObject.SetActive(value); }
	}

	public string LoadingText
	{
		get { return _text.text;  }	
		set { _text.text = value; }
	}


	#endregion

	//===============================================================

	#region Public Method

	public void Show(string whatIsBeingLoaded)
	{
		IsVisible = !string.IsNullOrEmpty(whatIsBeingLoaded);
		if (IsVisible)
		{
			ProgressRatio = 0f;
			LoadingText = string.Format(_loadTextFormat, whatIsBeingLoaded);
		}
	}
	
	#endregion

	//============================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();
		IsVisible = false;
	}

	protected override void OnDestroy()
	{
		_text	= null;
		_slider	= null;
		base.OnDestroy();
	}

	// Updates once per frame
	private void Update()
	{
        _text.color = new Color(
			_text.color.r, 
			_text.color.g, 
			_text.color.b, 
			Mathf.PingPong(Time.time, 1f));
	}

	#endregion


	//============================================

	#region Private Fields
	[SerializeField] private Text _text;
	[SerializeField] private Slider _slider;
	[SerializeField] private string _loadTextFormat = "Loading {0} ...";

	#endregion


}
