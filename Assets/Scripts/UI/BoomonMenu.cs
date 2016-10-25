using UnityEngine;

public class BoomonMenu : MonoBehaviour
{

	[SerializeField]
	private void OnBoomonSelected(BoomonController boomon)
	{
		MetaManager.Instance.GetManager<GameManager>().BoomonRole = boomon.Role;

		Camera.main.transform.LookAt(_roomMenu);
		boomon.GoTo(_roomMenu.transform.position);
	}


	private void Awake()
	{
		
	}

	private void OnDestroy()
	{
		_roomMenu = null;
	}


	[SerializeField] private Transform _roomMenu;
}
