using System.Collections.Generic;
using UnityEngine;

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
		OnObjectTapStart(FindObjectAtPosition(position), position);
	}

	protected override void OnTapStop(Vector2 position)
	{
		base.OnTapStop(position);
		OnObjectTapStop(FindObjectAtPosition(position), position);
	}

	
	protected override void OnTapStay(Vector2 position)
	{
		base.OnTapStay(position);
		OnObjectTapStay(FindObjectAtPosition(position), position);
	}

	protected override void OnDoubleTap(Vector2 position)
	{
		base.OnDoubleTap(position);
		OnObjectDoubleTap(FindObjectAtPosition(position), position);
	}
	
	protected override void OnSwipe(Vector2 position, Vector2 swipeVector, float speedRatio)
	{
		base.OnSwipe(position, swipeVector, speedRatio);
		OnObjectSwipe(FindObjectAtPosition(position), position, swipeVector, speedRatio);
	}

	//----------------------------------------------------------

	/// <summary>
	/// 
	/// </summary>
	/// <param name="o"></param>
	private void OnObjectTapStart(GameObject o, Vector2 position)
	{
		if (o != null)
		{
			IObjectTouchListener listener = o.GetComponent<IObjectTouchListener>();
			if (listener != null && _selfListeners.Contains(listener)) 
				listener.OnTapStart(o, position);
		}
	
		for(int i = _broadcastListeners.Count - 1; i >= 0; --i)
			_broadcastListeners[i].OnTapStart(o, position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="o"></param>
	private void OnObjectTapStay(GameObject o, Vector2 position)
	{
		if (o != null)
		{
			IObjectTouchListener listener = o.GetComponent<IObjectTouchListener>();
			if (listener != null && _selfListeners.Contains(listener))
				listener.OnTapStay(o, position);
		}

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			_broadcastListeners[i].OnTapStay(o, position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="o"></param>
	private void OnObjectTapStop(GameObject o, Vector2 position)
	{
		if (o != null)
		{
			IObjectTouchListener listener = o.GetComponent<IObjectTouchListener>();
			if (listener != null && _selfListeners.Contains(listener))
				listener.OnTapStop(o, position);
		}

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			_broadcastListeners[i].OnTapStop(o, position);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="o"></param>
	private void OnObjectDoubleTap(GameObject o, Vector2 position)
	{
		if (o != null)
		{
			IObjectTouchListener listener = o.GetComponent<IObjectTouchListener>();
			if (listener != null && _selfListeners.Contains(listener))
				listener.OnDoubleTap(o, position);
		}

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			_broadcastListeners[i].OnDoubleTap(o, position);
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="o"></param>
	/// <param name="position"></param>
	/// <param name="direction"></param>
	/// <param name="speedRatio"></param>
	private void OnObjectSwipe(GameObject o, Vector2 position, Vector2 direction, float speedRatio)
	{
		if (o != null)
		{
			IObjectTouchListener listener = o.GetComponent<IObjectTouchListener>();
			if (listener != null && _selfListeners.Contains(listener))
				listener.OnSwipe(o, position, direction, speedRatio);
		}

		for (int i = _broadcastListeners.Count - 1; i >= 0; --i)
			_broadcastListeners[i].OnSwipe(o, position, direction, speedRatio);
	}

	#endregion


	//==============================================

	#region Private Methods

	private GameObject FindObjectAtPosition(Vector2 position)
	{
		Ray ray = Camera.main.ScreenPointToRay(position);

		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, _objectDistanceMax, _touchLayerMask))
			return hitInfo.collider.gameObject;
		else
			return null;
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
