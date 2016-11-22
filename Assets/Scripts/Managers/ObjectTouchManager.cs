using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.Touch;

/// <summary>
/// 
/// </summary>
public class ObjectTouchManager : TouchManager
{
	#region Public Fields
	#endregion

	//===========================================================================

	#region Public Methods

	public void AddListener(IObjectTouchListener listener, bool onlyMyEvents = false)
	{
		if (onlyMyEvents)
			_selfListeners.Add(listener);
		else
			_broadcastListeners.Add(listener);
	}


	public void RemoveListener(IObjectTouchListener listener)
	{
		_broadcastListeners.Remove(listener);
		_selfListeners.Remove(listener);
	}

	#endregion

	//========================================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();

		_touchLayerMask = LayerMask.GetMask(_touchLayerName);
	}

	protected override void OnDestroy()
	{
		_selfListeners.Clear();
		_broadcastListeners.Clear();

		base.OnDestroy();
	}

	#endregion

	//=================================================================================

	#region Events 

	protected override void OnTapStart(Vector2 position)
	{
		base.OnTapStart(position);
		OnObjectTapStart(FindToucherAtPosition(position), position);
	}

	protected override void OnTapStop(Vector2 position)
	{
		base.OnTapStop(position);
		OnObjectTapStop(FindToucherAtPosition(position), position);
	}

	
	protected override void OnTapStay(Vector2 position)
	{
		base.OnTapStay(position);
		OnObjectTapStay(FindToucherAtPosition(position), position);
	}

	protected override void OnDoubleTap(Vector2 position)
	{
		base.OnDoubleTap(position);
		OnObjectDoubleTap(FindToucherAtPosition(position), position);
	}
	
	protected override void OnSwipe(Vector2 position, Vector2 swipeVector, float speedRatio)
	{
		base.OnSwipe(position, swipeVector, speedRatio);
		OnObjectSwipe(FindToucherAtPosition(position), position, swipeVector, speedRatio);
	}

	//----------------------------------------------------------

	/// <summary>
	/// 
	/// </summary>
	/// <param name="toucher"></param>
	/// <param name="position"></param>
	private void OnObjectTapStart(Toucher toucher, Vector2 position)
	{
		Log("OnObjectTapStart");
		
		if (toucher != null) 
			toucher.OnTapStart(toucher, position);

		for(int i = _broadcastListeners.Count - 1; i >= 0; --i)
			if(_broadcastListeners[i] != toucher)
				_broadcastListeners[i].OnTapStart(toucher, position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="toucher"></param>
	/// <param name="position"></param>
	private void OnObjectTapStay(Toucher toucher, Vector2 position)
	{ 
		if(toucher != null)
			toucher.OnTapStay(toucher, position);

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			if(_broadcastListeners[i] != toucher)
				_broadcastListeners[i].OnTapStay(toucher, position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="toucher"></param>
	/// <param name="position"></param>
	private void OnObjectTapStop(Toucher toucher, Vector2 position)
	{
		Log("OnObjectTapStop");
	 
		if(toucher != null)
			toucher.OnTapStop(toucher, position);

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			if(_broadcastListeners[i] != toucher)
				_broadcastListeners[i].OnTapStop(toucher, position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="toucher"></param>
	/// <param name="position"></param>
	private void OnObjectDoubleTap(Toucher toucher, Vector2 position)
	{
		Log("OnObjectDoubleTap");	
	
		if(toucher != null)
			toucher.OnDoubleTap(toucher, position);

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			if(_broadcastListeners[i] != toucher)
				_broadcastListeners[i].OnDoubleTap(toucher, position);
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="toucher"></param>
	/// <param name="position"></param>
	/// <param name="direction"></param>
	/// <param name="speedRatio"></param>
	private void OnObjectSwipe(Toucher toucher, Vector2 position, Vector2 direction, float speedRatio)
	{
		if(!_selfListeners.Contains(toucher) && !_broadcastListeners.Contains(toucher))
			toucher = null;

		if(toucher != null)
			toucher.OnSwipe(toucher, position, direction, speedRatio);

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			if(_broadcastListeners[i] != toucher)
				_broadcastListeners[i].OnSwipe(toucher, position, direction, speedRatio);
	}

	#endregion


	//==============================================

	#region Private Methods

	private Toucher FindToucherAtPosition(Vector2 position)
	{
		Toucher toucher = null;

		Ray ray = Camera.main.ScreenPointToRay(position); 
		RaycastHit hitInfo;	  
		if (Physics.Raycast(ray, out hitInfo, _objectDistanceMax, _touchLayerMask))
			toucher = hitInfo.collider.gameObject.GetComponent<Toucher>();

		if(!_selfListeners.Contains(toucher) && !_broadcastListeners.Contains(toucher))
			toucher = null;

		return toucher;
	}

	#endregion

	//==============================================


	#region Private Fields

	[SerializeField, Range(1f, 500f)] private float _objectDistanceMax = 100f;
	[SerializeField] private string _touchLayerName = "Touchable";

	private readonly HashSet<IObjectTouchListener> _selfListeners = new HashSet<IObjectTouchListener>();
	private readonly List<IObjectTouchListener> _broadcastListeners = new List<IObjectTouchListener>();
	
	private int _touchLayerMask;
	

	#endregion

}
