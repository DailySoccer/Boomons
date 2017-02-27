using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowOnClick : MonoBehaviour {

	public Transform target;
	public float maxHeight;

	void OnMouseDown() {
		MetaManager.Instance.Get<GameManager>().Boomon.Throw(target, maxHeight);
	}
}
