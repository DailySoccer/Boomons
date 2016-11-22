using System;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class Toucher : MonoBehaviour, IObjectTouchListener
{

	#region Public Methods

	public bool MustReceiveOnlyItsTouches {
		get { return _mustReceiveOnlyItsTouches; }
		set { _mustReceiveOnlyItsTouches = value; }
	}

	public virtual bool IsTouchEnabled 
	{
		get { return _isTouchEnabled; }
		set 
		{
			if(value == _isTouchEnabled)
				return;
			_isTouchEnabled = value;

			if(value)
				TouchManager.AddListener(this, _mustReceiveOnlyItsTouches);
			else
				TouchManager.RemoveListener(this);
		}
	}

	[Serializable] public class TapEvent	: UnityEvent<Toucher, Vector2> {}
	[Serializable] public class SwipeEvent	: UnityEvent<Toucher, Vector2, Vector2, float> {}

	public TapEvent TapStart	{ get { return _tapStart;	} }
	public TapEvent TapStop		{ get { return _tapStop;	} }
	public TapEvent TapStay		{ get { return _tapStay;	} }
	public TapEvent DoubleTap	{ get { return _doubleTap;	} }
	public SwipeEvent Swipe		{ get { return _swipe;		} }

	public virtual void OnTapStart(Toucher toucher, Vector2 touchPos)
	{
		_tapStart.Invoke(toucher, touchPos);
	}

	public virtual void OnTapStop(Toucher toucher, Vector2 position)
	{
		_tapStop.Invoke(toucher, position);
	}

	public virtual void OnTapStay(Toucher toucher, Vector2 position)
	{
		_tapStay.Invoke(toucher, position);
	}

	public virtual void OnDoubleTap(Toucher toucher, Vector2 position)
	{
		_doubleTap.Invoke(toucher, position);
	}

	public virtual void OnSwipe(Toucher toucher, Vector2 position, Vector2 direction, float speedRatio)
	{
		_swipe.Invoke(toucher, position, direction, speedRatio);
	}

	#endregion

	//===============================================


	#region Mono

	protected virtual void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(_touchLayerName);	 
	}

	protected virtual void OnDestroy()
	{
		_touchManager = null;
	}

	protected virtual void OnEnable()
	{
		IsTouchEnabled = true;  
	}

	protected virtual void OnDisable()
	{
		IsTouchEnabled = false;
	}

	#endregion

	//===================================================


	#region Private Fields

	private ObjectTouchManager TouchManager
	{
		get {
			return _touchManager ?? 
			 (_touchManager = MetaManager.Instance.Get<ObjectTouchManager>());
		}
	}

	
	[SerializeField]
	private string _touchLayerName = "Touchable";
	[SerializeField]
	private bool _mustReceiveOnlyItsTouches = true;

	[SerializeField]
	private TapEvent _tapStart = new TapEvent();
	[SerializeField]
	private TapEvent _tapStop = new TapEvent();
	[SerializeField]
	private TapEvent _tapStay = new TapEvent();
	[SerializeField]
	private TapEvent _doubleTap = new TapEvent();
	[SerializeField]
	private SwipeEvent _swipe = new SwipeEvent();

									   

	private bool _isTouchEnabled;
	private ObjectTouchManager _touchManager;

	#endregion

}

