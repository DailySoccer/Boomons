
using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BoomonCamera : BoomonFollower
{
	#region Public Fields

	[Serializable]
	public class  Modifiers : Setup 
	{
		public float FieldOfView	{ get { return _fieldOfView; } }

		[SerializeField, Range(1f, 180f)] private float _fieldOfView = 60f;

		public Modifiers() { }

		public Modifiers(
			Vector3 distance,
			float stopDistanceRatio = .3f,
			float depthSpeed = 3f,
			float lateralSpeed = 15f,
			float fieldOfView = 60f)	   :
			base(distance, stopDistanceRatio, depthSpeed, lateralSpeed)
		{
			_fieldOfView	  = fieldOfView;
		}
		
	}

	#endregion


	//=========================================================================

	#region Public Methods

	public void SetModifiers(Modifiers modifiers)
	{	  
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

	protected override void OnDestroy()
	{
		_camera = null;
		base.OnDestroy();
	}

	protected virtual void OnEnable()
	{
		_backupSetup = new Modifiers(
			CurrentSetup.Distance,
			CurrentSetup.StopDistanceRatio,
			CurrentSetup.DepthSpeed,
			CurrentSetup.LateralSpeed,
			_camera.fieldOfView
		);
	}

#if UNITY_EDITOR
	protected virtual void Update()
	{
		Debug.DrawLine(transform.position, Target.position, Color.blue);
	}
#endif

	//==============================================================

	#region Private Methods

	private void ApplyModifiers(Modifiers modifiers)
	{
		CurrentSetup = modifiers;
		if(_camera != null)
			_camera.fieldOfView = modifiers.FieldOfView;
	}

	#endregion

	//=================================================================

	#region Private Fields

	private Modifiers _backupSetup;
	private Camera _camera;

	#endregion

}

