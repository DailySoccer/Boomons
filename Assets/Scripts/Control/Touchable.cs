
using UnityEngine;


[RequireComponent(typeof(Collider))]
public abstract class Touchable : MonoBehaviour, IObjectTouchListener
{

	#region Public Methods
	public virtual void OnTapStart(GameObject go, Vector2 touchPos)
	{
	}

	public virtual void OnTapStop(GameObject go, Vector2 position)
	{
	}

	public virtual void OnTapStay(GameObject go, Vector2 position)
	{
	}

	public virtual void OnDoubleTap(GameObject go, Vector2 position)
	{
	}

	public virtual void OnSwipe(GameObject go, Vector2 position, Vector2 direction, float speedRatio)
	{
	}
	#endregion

	//===============================================


	#region Mono

	protected virtual void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(_touchLayerName);
		_touchManager = TouchManager.GetInstance<ObjectTouchManager>();
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

	protected bool IsTouchEnabled
	{
		get { return _isInputEnabled; }
		set
		{
			if (value == _isInputEnabled)
				return;
			_isInputEnabled = value;

			if(value)
				_touchManager.AddListener(this, _mustReceiveOnlyItsTouches);
			else
				_touchManager.RemoveListener(this);
		}
	}


	[SerializeField] private string _touchLayerName = "Touchable";
	[SerializeField] private bool _mustReceiveOnlyItsTouches = true;

	private ObjectTouchManager _touchManager;
	private bool _isInputEnabled;

	#endregion

}

