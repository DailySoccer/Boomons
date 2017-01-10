using UnityEngine;

[CreateAssetMenu(fileName = "RagdollSetup", menuName = "Setup/Ragdoll", order=101)]
public class RagdollSetup : ScriptableObject
{

	#region Public Fields

	public float StopVelocityMaxSqr { get { return _stopVelocityMaxSqr; } }
	public float TimeoutSecs { get { return _timeoutSecs; } }

	public float GroundCosine {
		get {
			return _groundCosine ??
				(_groundCosine = Mathf.Cos(_groundDegreesMax * Mathf.Deg2Rad)).Value;
		}
	}

	public int Layer {
		get {
			return _layer > 0 ? _layer :
				(_layer = LayerMask.NameToLayer(_layerName));
		}
	}

	#endregion

	//=======================================================================


	#region Mono

	//private void OnEnable()
	//{

	//}


	//private void OnDisable()
	//{

	//}


	//private void OnDestroy()
	//{

	//}

	#endregion


	//===================================================


	#region Private Fields

	[SerializeField] private string _layerName = "Ground";
	[SerializeField, Range(0.01f, 5f)] private float  _stopVelocityMaxSqr = .1f;
	[SerializeField, Range(0.1f, 5f)]  private float  _timeoutSecs = 1f;
	[SerializeField, Range(0f, 90f)]   private float  _groundDegreesMax = 45f;

	private int _layer;
	private float? _groundCosine;
	
	#endregion


}
