using UnityEngine;

public class BoomonMenu : MonoBehaviour
{
	

	#region Public Methods

	public void OnBoomonSelected(BoomonController boomon)
	{
		MetaManager.Instance.GetManager<GameManager>().BoomonRole = boomon.Role;

		_boomonAnimator = boomon.GetComponent<Animator>();
		_boomonAnimator.SetTrigger("StandUp");
		_boomonAnimator.GetBehaviour<BoomonIdleState>().Start += OnBoomonIdleReady;

		
	}

	#endregion

	//======================================================================

	#region Mono

	private void OnDisable()
	{
		// TODO FRS 161027 Desuscribir eventos
		//if(_boomonAnimator != null)
		//	_boomonAnimator.GetBehaviour<BoomonIdleState>().Start -= OnBoomonIdleReady;
	}

	private void OnDestroy()
	{
		_boomonAnimator = null;
	}

	#endregion


	//==============================================================


	#region Events

	public void OnBoomonIdleReady()
	{
		_boomonAnimator.SetTrigger("Tickles");
		Camera.main.GetComponent<Animator>().SetTrigger("ShowRooms");
	}

	#endregion		 

	//==============================================================

	#region Private Fields
	private Animator _boomonAnimator;
	#endregion
}
