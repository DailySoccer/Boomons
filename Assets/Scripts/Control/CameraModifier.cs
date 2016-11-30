using UnityEngine;

public class CameraModifier : BoomonProximityDetector
{  
	  // TODO Habilitar con la distancia
	#region Mono

	protected  override void Start()
	{
		base.Start();
		_camera = MetaManager.Instance.Get<GameManager>().Camera;
	}

	protected override void OnDestroy()
	{
		_camera = null;
		base.OnDestroy();
	}


	#endregion

	//=======================================================================

	#region Events

	protected override void OnTargetEnter()
	{
		base.OnTargetEnter();
		Debug.Log("CameraModifier::OnTargetEnter>> " + name, this);
		if(_camera != null)
			_camera.SetModifiers(_modifiers);
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

	[SerializeField] private BoomonCamera.Modifiers _modifiers;

	private BoomonCamera _camera;

	#endregion

}
