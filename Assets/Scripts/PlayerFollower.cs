using UnityEngine;
using System.Collections;

public class PlayerFollower : MonoBehaviour {

	// Use this for initialization
	private void Start ()
	{
		if (_boomon == null)
			_boomon = FindObjectOfType<BoomonController>();
	}

	private void OnDestroy()
	{
		_boomon = null;
	}
	
	private void LateUpdate ()
	{
		transform.position = _boomon.Position 
			- _distance * transform.forward
			+ _height * transform.up;
	}



	[SerializeField] private BoomonController _boomon;
	[SerializeField, Range(1f, 10f)] private float _distance = 5f;
	[SerializeField, Range(0f, 2f)] private float _height  = 2f;
}
