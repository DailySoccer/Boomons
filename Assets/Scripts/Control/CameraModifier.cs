using UnityEngine;

public class CameraModifier : BoomonProximityDetector
{  
	  // TODO Habilitar con la distancia
	#region Mono

	protected void OnDisable()
	{
		if(IsTargetNearby)	
			OnTargetExit();
	}	

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_camera = null;
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
		if (!enabled)
			return;

		base.OnTargetEnter();
		Debug.Log("CameraModifier::OnTargetEnter>> " + name, this);
		Camera.SetModifiers(_modifiers);
	}

	protected override void OnTargetExit()
	{	
		base.OnTargetExit();
		Debug.Log("CameraModifier::OnTargetExit>> " + name, this);
		Camera.ClearModifiers();
	}

	#endregion

	//===========================================================================

	#region Private Fields

	private BoomonCamera Camera {
		get {
			return _camera ??
				(_camera = MetaManager.Instance.Get<GameManager>().Camera);
		}
	}


	[SerializeField] private BoomonCamera.Modifiers _modifiers;

	private BoomonCamera _camera;

	#endregion

}
