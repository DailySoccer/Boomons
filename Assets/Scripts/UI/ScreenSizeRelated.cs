using UnityEngine;
using UnityEngine.UI;

public class ScreenSizeRelated : MonoBehaviour {

	[SerializeField]
	[Range(0, 1)]
	private float Percentage = 0.2f;
	// Use this for initialization
	void Start () {
		Sprite refSprite = GetComponentInChildren<Image>().sprite;
		float ratioSprite = refSprite != null ? refSprite.rect.width / refSprite.rect.height : 1;
		float shortest = Mathf.Min(Screen.width, Screen.height);
		RectTransform myRect = GetComponent<RectTransform>();
		if (myRect != null)
		{
			myRect.sizeDelta = new Vector2(ratioSprite , 1) * shortest * Percentage;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
