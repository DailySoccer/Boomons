using UnityEngine;
using System.Collections;

public class AdjustToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RectTransform mine = gameObject.GetComponent<RectTransform>();
		if (mine != null)
		{
			float screenSize = Mathf.Min(Screen.width, Screen.height);
			mine.sizeDelta = new Vector2(screenSize, screenSize);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
