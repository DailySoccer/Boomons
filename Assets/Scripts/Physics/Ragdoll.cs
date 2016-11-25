using System;
using UnityEngine;

public class Ragdoll : MonoBehaviour, IObjectTouchListener, ITeleportable
{

	#region Public Fields

	public event Action<Vector3> GroundEnter;

	public Transform Transform { get { return _pelvis.transform; } }
	public GroundDetectionParams GroundParams { get { return _groundParams; } }

	public bool IsTeleporting { get; private set; }

	public bool IsTouchEnabled 
	{
		get { return _pelvis.IsTouchEnabled; }
		set
		{
			_pelvis.IsTouchEnabled = value;
			if (value)
				return;

			foreach(Rigidbody r in GetComponentsInChildren<Rigidbody>())
				r.velocity = Vector3.zero;
		}
	}

	public ReferenceSystem ReferenceSystem { get { return _refSystem;  } }

	#endregion

	//========================================================================

	#region Public Methods

	public virtual void Setup(Transform setupRef, ReferenceSystem  refSystem = null)
	{
		IsTeleporting = false;

		_refSystem = refSystem;

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

	public virtual void Throw(Vector3 velocity, Vector3? applyPosition = null)
	{
		_pelvis.Throw(velocity, applyPosition);
	}

	public virtual void OnTapStart(Toucher toucher, Vector2 position)
	{
	}

	public virtual void OnTapStop(Toucher toucher, Vector2 position)
	{
	}

	public virtual void OnTapStay(Toucher toucher, Vector2 position)
	{
	}

	public virtual void OnDoubleTap(Toucher toucher, Vector2 position)
	{
	}

	public virtual void OnSwipe(Toucher toucher, Vector2 position, Vector2 direction, float speedRatio)
	{
		_pelvis.OnSwipe(toucher, position, direction, speedRatio);
	}

	public virtual void TeleportTo(Teleport target)
	{
		IsTeleporting = true;
	}


	#endregion


	//==================================================================


	#region Mono

	protected virtual void Awake()
	{
		_nodes = GetComponentsInChildren<Transform>(true);
		if (_pelvis == null)
			_pelvis = GetComponentInChildren<RagdollPelvis>();

		_pelvis.Ragdoll = this;
	
		gameObject.SetActive(false);
	}

	protected virtual void OnDestroy()
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

		IsTouchEnabled = true;
	}

	#endregion


	//=================================================================

	#region Private Methods

	#endregion

	//==============================================================

	#region Private Fields


	[SerializeField] private GroundDetectionParams _groundParams;
	
	[Serializable]
	public class GroundDetectionParams
	{
		public float StopVelocityMaxSqr { get { return _stopVelocityMaxSqr; } }
		public float Timeout			{ get { return _timeoutSecs;		} }

		public float GroundCosine {
			get {
				return _groundCosine ?? 
					(_groundCosine = Mathf.Cos(_groundDegreesMax*Mathf.Deg2Rad)).Value;
			}
		}
		
		public int Layer {
			get {
				return _layer > 0 ?  _layer :
					(_layer = LayerMask.NameToLayer(_layerName));
			}
		}
			
		[SerializeField, Range(0.01f, 5f)] private float  _stopVelocityMaxSqr = .1f;
		[SerializeField, Range(0.1f, 5f)]  private float  _timeoutSecs = 1f;
		[SerializeField]				   private string _layerName = "Ground";
		[SerializeField, Range(0f, 90f)]   private float  _groundDegreesMax = 45f;

		private int _layer;
		private float? _groundCosine;
	}


	private RagdollPelvis _pelvis;
	private Transform[] _nodes;

	private string _eyebrowKeyWord = "Eyebrow";
	private ReferenceSystem _refSystem;

	#endregion


}
