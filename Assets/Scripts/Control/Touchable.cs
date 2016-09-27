
using UnityEngine;



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

	public virtual void OnSwipe(GameObject go, Vector2 swipeVector, float speedRatio)
	{
	}
	#endregion

	//===============================================


	#region Mono

	protected virtual void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(_touchLayerName);
		_touchManager = ObjectTouchManager.Instance;
	}

	protected virtual void OnDestroy()
	{
		_touchManager = null;
	}

	protected virtual void OnEnable()
	{
		_touchManager.AddListener(this, _mustReceiveOnlyItsTouches);
	}

	protected virtual void OnDisable()
	{
		_touchManager.RemoveListener(this);
	}

	#endregion



	//===================================================


	#region Private Fields

	[SerializeField] private string _touchLayerName = "Touchable";
	[SerializeField] private bool _mustReceiveOnlyItsTouches = true;

	private ObjectTouchManager _touchManager;
	
	#endregion

}

