using UnityEngine;
using System.Collections;

public class ScreenSizeRelated : MonoBehaviour {

	[SerializeField]
	[Range(0, 1)]
	private float Percentage = 0.2f;
	// Use this for initialization
	void Start () {
		float shortest = Mathf.Min(Screen.width, Screen.height);
		RectTransform myRect = GetComponent<RectTransform>();
		if (myRect != null)
		{
			myRect.sizeDelta = Vector2.one * shortest * Percentage;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
