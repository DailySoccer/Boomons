using UnityEngine;

public class BoomonMenu : MonoBehaviour
{
	#region Public Methods

	public void OnBoomonSelected(BoomonController boomon)
	{
		MetaManager.Instance.GetManager<GameManager>().BoomonRole = boomon.Role;
	}

	#endregion

	//======================================================================

	#region Mono

	private void OnIdleReady()
	{
		Debug.Log(1);	
	}

	#endregion

}
