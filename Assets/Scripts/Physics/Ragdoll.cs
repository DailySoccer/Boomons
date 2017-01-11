using System;
using UnityEngine;

public class Ragdoll : MonoBehaviour, IObjectTouchListener, ITeleportable
{

	#region Public Fields

	public event Action<Vector3> GroundEnter;

	public Transform Transform { get { return _pelvis.transform; } }


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

	public RagdollSetup Setup { get; private set; }
	public ReferenceSystem ReferenceSystem { get; private set; }


	#endregion

	//========================================================================

	#region Public Methods

	public virtual void Init(Transform refTransform, RagdollSetup setup, ReferenceSystem  refSystem = null)
	{
		Setup = setup;
		ReferenceSystem = refSystem;
		IsTeleporting = false;

		_pelvis.Init(this);

		var refNodes = refTransform.GetComponentsInChildren<Transform>(true);
		for(int i = 0; i < refNodes.Length; ++i)
		{
			Debug.Assert(i == 0 || _nodes[i].name == refNodes[i].name, 
				"Ragdoll::Init>> Hierarchy does not match: " 
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


	private RagdollPelvis _pelvis;
	private Transform[] _nodes;

	private string _eyebrowKeyWord = "Eyebrow";

	#endregion


}
