using UnityEngine;

public class PathSolver : Singleton<PathSolver>
{

	#region Public Fields

	public enum InstanceType
	{
		Controllable = 0,
		NotControllable = 1,
		Ragdoll = 2
	}

	#endregion


	//=======================================================


	#region Public Methods

	public string GetBoomonPath(BoomonRole role, InstanceType instanceType)
	{
		return string.Format(_boomonPathFormat, instanceType, role);
	}

	#endregion


	//======================================================================


	#region Private Fields

	[SerializeField] private string _boomonPathFormat = "Boomons/{0}/{1}";

	#endregion
}
