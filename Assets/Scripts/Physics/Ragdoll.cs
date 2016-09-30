using System;
using UnityEngine;

public class Ragdoll : MonoBehaviour, IObjectTouchListener
{

	#region Public Fields

	public event Action<Vector3> GroundEnter;

	public Vector3 Position {  get { return _pelvis.transform.position;  } }
	public int GroundLayer { get; private set; }
	public float StopVelocityMaxSqr { get { return _stopVelocityMaxSqr; } }

	#endregion

	//========================================================================

	#region Public Methods

	public void Setup(Transform setupRef)
	{
		var refNodes = setupRef.GetComponentsInChildren<Transform>(true);
		for(int i = 0; i < refNodes.Length; ++i)
		{
			Debug.Assert(i == 0 || _nodes[i].name == refNodes[i].name, 
				"Ragdoll::Setup>> Hierarchy does not match: " 
				+ _nodes[i].name + " VS " + refNodes[i].name, gameObject);

			_nodes[i].localPosition = refNodes[i].localPosition;
			_nodes[i].localRotation = refNodes[i].localRotation;
		}

		gameObject.SetActive(true);
	}


	public void OnTapStart(GameObject go, Vector2 position)
	{
	}

	public void OnTapStop(GameObject go, Vector2 position)
	{
	}

	public void OnTapStay(GameObject go, Vector2 position)
	{
	}

	public void OnDoubleTap(GameObject go, Vector2 position)
	{
	}

	public void OnSwipe(GameObject go, Vector2 swipeVector, float speedRatio)
	{
		_pelvis.OnSwipe(go, swipeVector, speedRatio);
	}

	#endregion


	//==================================================================


	#region Mono

	protected void Awake()
	{
		GroundLayer = LayerMask.NameToLayer(_groundLayerName);

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

		gameObject.SetActive(false);
	}

	#endregion


	//=================================================================

	#region Private Methods

	#endregion

	//==============================================================

	#region Private Fields

	[SerializeField] private RagdollPelvis _pelvis;
	[SerializeField, Range(0.01f, 5f)]
	private float _stopVelocityMaxSqr = 0.1f;
	[SerializeField]
	private string _groundLayerName = "Ground";

	private Transform[] _nodes;

	#endregion


}
