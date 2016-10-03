using System;
using UnityEngine;



/// <summary>
/// 
/// </summary>
public class TouchManager : Singleton<TouchManager>
{
	private struct InputData
	{
		public enum InputPhase
		{
			None   = 0,
			Start  = 1,
			Stay   = 2,
			Stop   = 3
		}

		public Vector2 Position;
		public float Seconds;
		public InputPhase Phase;


		private InputData(Vector2 position, float seconds, InputPhase phase = InputPhase.None)
		{
			Phase	 = phase;
			Position = position;
			Seconds	 = seconds;
		}

		public static InputData operator-(InputData x, InputData y)
		{
			return new InputData(x.Position - y.Position, x.Seconds - y.Seconds);
		}
	}

	#region Public members

	/// <summary>
	/// Returns swipe vector in inches/sec
	/// </summary>
	public event Action<Vector2, Vector2, float> Swipe;

	/// <summary>
	/// Returns double tap position in inches
	/// </summary>
	public event Action<Vector2> DoubleTap;

	public event Action<Vector2> TapStart;
	public event Action<Vector2> TapStop;
	public event Action<Vector2> TapStay;

	#endregion

	//==============================================================================

	#region MonoBehaviour methods

	protected override void Awake()
	{
		_dpiSqr = Screen.dpi*Screen.dpi;
	}
	/// <summary>
	/// 
	/// </summary>
	protected override void OnDestroy()
	{
		Swipe = null;
		DoubleTap = null;
		TapStay = null;

		base.OnDestroy();
	}

	/// <summary>
	/// 
	/// </summary>
	private void Update()
	{
		InputData input = ReadInput();

		switch (input.Phase)
		{
			case InputData.InputPhase.Start:						
				OnInputStart(input);
				break;

			case InputData.InputPhase.Stop:
				OnInputStop(input);
				break;

			case InputData.InputPhase.Stay:
				OnInputStay(input);
				break;
					
			default:
				OnNoInputStay();
				break;
		}
	}


	#endregion


	//======================================================================


	#region Events

	/// <summary>
	/// 
	/// </summary>
	/// <param name="input"></param>
	private void OnInputStart(InputData input)
	{
		if (_beginInput.HasValue && _endInput.HasValue)
			CheckDoubleTap(_beginInput.Value, input);

		_beginInput = input;
		_endInput = null;
		OnTapStart(input.Position);
	}



	/// <summary>
	/// 
	/// </summary>
	/// <param name="input"></param>
	private void OnInputStop(InputData input)
	{
		OnTapStop(input.Position);

		if (!_beginInput.HasValue)
			return;

		_endInput = input;
		CheckSwipe(_beginInput.Value, _endInput.Value);
	}




	/// <summary>
	/// 
	/// </summary>
	/// <param name="input"></param>
	private void OnInputStay(InputData input)
	{
		OnTapStay(input.Position);

		if (!_beginInput.HasValue)
			return;

		if (CheckSwipe(_beginInput.Value, input) == SwipeResultType.SpeedFail)
			_beginInput = input; // Refresco de fecha del input para que el cálculo de velocidad del swipe sea correcto
	}

	/// <summary>
	/// 
	/// </summary>
	private void OnNoInputStay()
	{
	}


	//---------------------------------------------------------------------

	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnSwipe(Vector2 position, Vector2 swipeVector, float speedRatio)
	{
		var e = Swipe;
		if (e != null)
			e(position, swipeVector, speedRatio);
	}

	/// <summary>
	/// 
	/// </summary>
	protected virtual void OnDoubleTap(Vector2 position)
	{
		_beginInput = null;
		_endInput = null;

		Debug.Log("DoubleTap>> " + position);

		var e = DoubleTap;
		if (e != null)
			e(position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="position"></param>
	protected virtual void OnTapStart(Vector2 position)
	{
		var e = TapStart;
		if (e != null)
			e(position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="position"></param>
	protected virtual void OnTapStop(Vector2 position)
	{
		var e = TapStop;
		if (e != null)
			e(position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="position"></param>
	protected virtual void OnTapStay(Vector2 position)
	{
		var e = TapStay;
		if (e != null)
			e(position);
	}

	#endregion

	//======================================================================

	#region Private methods


	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	private InputData ReadInput()
	{
#if UNITY_EDITOR
		InputData input;
		input.Phase =	Input.GetMouseButtonDown(0)	? InputData.InputPhase.Start :
						Input.GetMouseButtonUp(0)	? InputData.InputPhase.Stop  :
						Input.GetMouseButton(0)		? InputData.InputPhase.Stay :
													InputData.InputPhase.None;
		input.Position = Input.mousePosition;
		input.Seconds = Time.time;
#else
		if (Input.touchCount == 0)
			return new InputData();

		InputData input;
		Touch touch = Input.GetTouch(0);

		switch (touch.phase)
		{
			case TouchPhase.Began:
				input.Phase = InputData.InputPhase.Start;
				break;
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				input.Phase = InputData.InputPhase.Stop;
				break;
			default:
				input.Phase = InputData.InputPhase.Stay;
				break;
		}	
			
		input.Position = touch.position;
		input.Seconds = Time.time;
#endif
		return input;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="begin"></param>
	/// <param name="end"></param>
	private SwipeResultType CheckSwipe(InputData begin, InputData end)
	{
		InputData swipe = end - begin;

		SwipeResult result;
		if (IsSwipe(swipe, out result) ) 
			OnSwipe(begin.Position, swipe.Position, result.SpeedRatio);

		return result.Type;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="firstTap"></param>
	/// <param name="secondTap"></param>
	/// <returns></returns>
	private bool CheckDoubleTap(InputData firstTap, InputData secondTap)
	{
		if (IsDoubleTap(secondTap - firstTap)) {
			OnDoubleTap(firstTap.Position);
			return true;

		} else {
			return false;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	private bool IsDoubleTap(InputData data)
	{
		return data.Seconds < _doubleTapSecsMax && !IsSwipe(data);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	private bool IsSwipe(InputData data)
	{
		SwipeResult result;
		return IsSwipe(data, out result);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	private bool IsSwipe(InputData data, out SwipeResult result)
	{
		result = new SwipeResult();

		bool isSwipe = data.Position.sqrMagnitude > _swipeInchesSqrMin*_dpiSqr;
		if (!isSwipe)
		{
			result.Type = SwipeResultType.DistanceFail;
			return false;
		}

		float inchesPerSecSqr = data.Position.sqrMagnitude/(_dpiSqr*data.Seconds*data.Seconds);
		isSwipe &= inchesPerSecSqr > _swipeInchesPerSecMin*_swipeInchesPerSecMin;
		if (!isSwipe)
		{
			result.Type = SwipeResultType.SpeedFail;
			return false;
		}

		result.SpeedRatio = Mathf.Clamp01(Mathf.Sqrt(inchesPerSecSqr)/_swipeInchesPerSecMax);
		result.Type = SwipeResultType.Success;
		return true;
	}

	#endregion

	//==========================================================================

	#region Private members

	private enum SwipeResultType
	{
		Success = 0,
		DistanceFail = 1,
		SpeedFail = 2,
	}

	private struct SwipeResult
	{
		public SwipeResultType Type;
		public float SpeedRatio;
	}

	[SerializeField, Range(0.01f, 1f)]	private float _doubleTapSecsMax = 0.5f;
	[SerializeField, Range(0f, .5f)]	private float _swipeInchesSqrMin = .1f;
	[SerializeField, Range(0.1f, 100f)]	private float _swipeInchesPerSecMax = 40f;
	[SerializeField, Range(0f, 50f)]    private float _swipeInchesPerSecMin = 10f;

	private InputData? _beginInput;
	private InputData? _endInput;

	private float _dpiSqr;

	#endregion
}
