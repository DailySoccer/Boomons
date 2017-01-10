using UnityEngine;

[CreateAssetMenu(fileName = "BoomonSetup", menuName = "Setup/Boomon", order=101)]
public class BoomonSetup : ScriptableObject
{

	#region Public Fields


	public string MoveMultiplierParamName {
		get { return _moveMultiplierParamName; }
	}
	public float MoveSpeed {
		get { return _moveSpeed; }
	}
	public float MoveAnimMultiplierBase {
		get { return _moveAnimMultiplierBase; }
	}
	public float MoveAnimSpeed {
		get { return _moveSpeed * _moveAnimMultiplierBase; }
	}
	public float SenseReversalInchesMin {
		get { return _senseReversalInchesMin; }
	}

	public Vector3 Right {
		get { return _right; }
	}
	public float Bounciness {
		get { return _bounciness; }
	}
	public float PushMass {
		get { return _pushMass; }
	}
				
	public float ThrowDegreesMin {
		get { return _throwDegreesMin; }
	}
	public float FallHeightMin {
		get { return _fallHeightMin; }
	}

	public RagdollSetup RagdollSetup { get { return _ragdollSetup; } }

	#endregion

	//=======================================================================


	#region Mono

	//private void OnEnable()
	//{

	//}


	//private void OnDisable()
	//{

	//}


	private void OnDestroy()
	{
		_ragdollSetup = null;
	}

	#endregion


	//===================================================


	#region Private Fields

	[SerializeField, Range(0.5f, 50f)]	private float _moveSpeed = 2.5f;
	[SerializeField, Range(0f, 10f)]	private float _senseReversalInchesMin = 1f;
	[SerializeField]					private string _moveMultiplierParamName = "MoveMultiplier";
	[SerializeField, Range(0f, 1f)]		private float _moveAnimMultiplierBase = .4f;

	[SerializeField] private Vector3 _right;
	[SerializeField, Range(0f, 1f)]		private float _bounciness = .5f;
	[SerializeField, Range(0f, 20f)]	private float _pushMass = 5f;
	
	[SerializeField, Range(0f, 90f)]	private float _throwDegreesMin = 35f;
	[SerializeField, Range(0f, 100f)]	private float _fallHeightMin = 100f;

	[SerializeField] private RagdollSetup _ragdollSetup;

	#endregion


}
