using UnityEngine;

public class CameraModifier : BoomonProximityDetector
{  
	  // TODO Habilitar con la distancia
	#region Mono
							   
	protected void OnDisable()
	{
		ProximityTarget = null;
	}

	protected override void OnDestroy()
	{
		_camera = null;
		base.OnDestroy();
	}

#if UNITY_EDITOR
	private void Update()
	{
		if(IsTargetNearby)
			Camera.SetModifiers(_modifiers);
	}
#endif

#endregion

	//=======================================================================

	#region Events

	protected override void OnTargetEnter()
	{
		base.OnTargetEnter();
		Debug.Log("CameraModifier::OnTargetEnter>> " + name, this);
		Camera.SetModifiers(_modifiers);
	}

	protected override void OnTargetExit()
	{
		base.OnTargetExit();
		Debug.Log("CameraModifier::OnTargetExit>> " + name, this);
		if(_camera != null)
			_camera.ClearModifiers();
	}

	#endregion

	//===========================================================================

	#region Private Fields

	private BoomonCamera Camera
	{
		get {
			return _camera ??
				(_camera = MetaManager.Instance.Get<GameManager>().Camera);
		}
	}


	[SerializeField] private BoomonCamera.Modifiers _modifiers;

	private BoomonCamera _camera;

	#endregion

}
