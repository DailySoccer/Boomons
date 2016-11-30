
using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BoomonCamera : BoomonFollower
{
	#region Public Fields

	[Serializable]
	public class Modifiers
	{
		public float FieldOfView	{ get { return _fieldOfView; } }
		public Vector3 Distance		{ get { return _distance; } }
		public float StopZoomRatio	{ get { return _stopZoomRatio; } }
		public float DepthSpeed		{ get { return _depthSpeed; } }
		public float LateralSpeed	{ get { return _lateralSpeed; } }

		[SerializeField] private Vector3 _distance = new Vector3(0f, .8f, 4f);
		[SerializeField, Range(0f, 1f)]  private float _stopZoomRatio = .3f;
		[SerializeField, Range(0f, 20f)] private float _depthSpeed = 3f;
		[SerializeField, Range(0f, 20f)] private float _lateralSpeed = 15f;
		[SerializeField, Range(1f, 180f)] private float _fieldOfView = 60f;

		public Modifiers() { }

		public Modifiers(
			Vector3 distance,
			float stopZoomRatio = .3f,
			float depthSpeed = 3f,
			float lateralSpeed = 15f,
			float fieldOfView = 60f)
		{
			_fieldOfView	  = fieldOfView;
			_distance = distance;
			_stopZoomRatio = stopZoomRatio;
			_depthSpeed = depthSpeed;
			_lateralSpeed = lateralSpeed;
		}
		
	}

	#endregion


	//=========================================================================

	#region Public Methods

	public void SetModifiers(Modifiers modifiers)
	{
		_backupSetup = new Modifiers(
			Distance,
			StopDistanceRatio,
			DepthSpeed,
			LateralSpeed,
			_camera.fieldOfView
		);

		ApplyModifiers(modifiers);
	}

	public void ClearModifiers()
	{
		ApplyModifiers(_backupSetup);
	}



	#endregion

	//===========================================================

	protected override void Awake()
	{
		base.Awake();
		_camera = GetComponent<Camera>();
	}


	//==============================================================

	#region Private Methods

	private void ApplyModifiers(Modifiers modifiers)
	{
		_camera.fieldOfView = modifiers.FieldOfView;
		Distance = modifiers.Distance;
		StopDistanceRatio = modifiers.StopZoomRatio;
		DepthSpeed = modifiers.DepthSpeed;
		LateralSpeed = modifiers.LateralSpeed;
	}

	#endregion

	//=================================================================

	#region Private Fields

	private Modifiers _backupSetup;
	private Camera _camera;

	#endregion

}

