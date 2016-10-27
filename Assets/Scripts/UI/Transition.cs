using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Transition : MonoBehaviour {

	public delegate void VoidNoParams();
	public event VoidNoParams AnimEnd;

	[SerializeField]
	private RectTransform Center;
	[SerializeField]
	private RectTransform TopBorder;
	[SerializeField]
	private RectTransform BottomBorder;
	[SerializeField]
	private RectTransform LeftBorder;
	[SerializeField]
	private RectTransform RightBorder;

	[SerializeField]
	private CanvasGroup AlphaGroup;

	[SerializeField]
	private float TimePeriod;

	[SerializeField]
	private bool Reverse;
	// Use this for initialization
	void Start () {
		_initialized = AlphaGroup != null && Center != null && TopBorder != null && BottomBorder != null && LeftBorder != null && RightBorder != null;
		if (_initialized)
		{
			ResetValues();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_workToDo)
		{
			UpdatePerc();
			_value = GetCosValue01(Reverse ? 1 - _perc : _perc);
			_workToDo = _perc > 1;
			UpdateGraphics(_value);
			if (!_workToDo)
			{
				if (AnimEnd != null)
				{
					AnimEnd();
				}
			}
		}
	}

	public void StartAnim()
	{
		_startTime = Time.time;
		_workToDo = true;
		ResetValues();
	}

	private void UpdatePerc()
	{
		float deltaTime = Time.time - _startTime;
		_perc = deltaTime / TimePeriod;
	}

	private void ResetValues()
	{
		UpdateGraphics(Reverse ? 1 : 0);
	}

	private void UpdateGraphics(float value)
	{
		//TODO
	}

	private float GetCosValue01(float inputPerc)
	{
		float input = Mathf.Clamp01(inputPerc);
		float result = (1 + Mathf.Cos(Mathf.PI * (1 + input))) * 0.5f;
		return result;
	}

	private bool _initialized;
	private bool _workToDo;
	private float _startTime;
	private float _perc;
	private float _value;
}
