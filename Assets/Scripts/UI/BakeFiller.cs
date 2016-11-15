using UnityEngine;
using System.Collections;

public class BakeFiller : MonoBehaviour {

	public RectTransform leftBake;
	public RectTransform rightBake;
	// Use this for initialization
	void Start () {
		if (rightBake != null && leftBake != null)
		{
			Vector2 currentSize = leftBake.sizeDelta;
			RectTransform myRect = gameObject.GetComponent<RectTransform>();
			currentSize.x = (Screen.width - (myRect.anchorMax - myRect.anchorMin).y * Screen.height) * 0.5f;
			rightBake.sizeDelta = leftBake.sizeDelta = currentSize;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
