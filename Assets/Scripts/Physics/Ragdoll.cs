using System;
using UnityEngine;

public class Ragdoll : MonoBehaviour, IObjectTouchListener, ITeleportable
{

	#region Public Fields

	public event Action<Vector3> GroundEnter;

	public Vector3 Position {  get { return _pelvis.transform.position;  } }
	public GroundDetectParams GroundParams { get { return _groundParams; } }

	public bool IsTeleporting { get; private set; }
	#endregion

	//========================================================================

	#region Public Methods

	public virtual void Setup(Transform setupRef)
	{
		IsTeleporting = false;

		var refNodes = setupRef.GetComponentsInChildren<Transform>(true);
		for(int i = 0; i < refNodes.Length; ++i)
		{
			Debug.Assert(i == 0 || _nodes[i].name == refNodes[i].name, 
				"Ragdoll::Setup>> Hierarchy does not match: " 
				+ _nodes[i].name + " VS " + refNodes[i].name, gameObject);

			if (!_nodes[i].name.Contains(_eyebrowKeyWord)) { // TODO FRS 161027 Mejorar con hashSet configurable (blacklist)
				_nodes[i].localPosition=refNodes[i].localPosition;
				_nodes[i].localRotation=refNodes[i].localRotation;
			}
		}

		gameObject.SetActive(true);
	}


	public virtual void OnTapStart(GameObject go, Vector2 position)
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
		_pelvis.OnSwipe(go, position, direction, speedRatio);
	}

	public virtual void TeleportTo(Teleport target)
	{
		IsTeleporting = true;
	}


	#endregion


	//==================================================================


	#region Mono

	protected void Awake()
	{
		_nodes = GetComponentsInChildren<Transform>(true);
		if (_pelvis == null)
			_pelvis = GetComponentInChildren<RagdollPelvis>();

		_pelvis.Ragdoll = this;
	
		gameObject.SetActive(false);
	}

	protected void OnDestroy()
	{
		_pelvis = null;
		_nodes = null;
	}

	#endregion


	//====================================================================


	#region Events

	public void OnGroundEnter(Vector3 groundPos)
	{
		var e = GroundEnter;
		if (e != null)
			e(groundPos);
	}

	#endregion


	//=================================================================

	#region Private Methods

	#endregion

	//==============================================================

	#region Private Fields

	
	[SerializeField] private GroundDetectParams _groundParams;
	[Serializable]
	public struct GroundDetectParams
	{
		[SerializeField, Range(0.01f, 5f)] public float  StopVelocityMaxSqr;
		[SerializeField, Range(0.1f, 5f)]  public float  Timeout;
		[SerializeField]				   public string LayerName;

		private int _layer;
		public int Layer {
			get {
				return _layer > 0 ?  _layer :
					(_layer = LayerMask.NameToLayer(LayerName));
			}
		}
	}


	private RagdollPelvis _pelvis;
	private Transform[] _nodes;

	private string _eyebrowKeyWord = "Eyebrow";

	#endregion


}
