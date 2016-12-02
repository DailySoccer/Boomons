using UnityEngine;
using System.Collections;

public class AdjustToScreen : MonoBehaviour {

	public Sprite refTexture;
	// Use this for initialization
	void Start () {
		RectTransform mine = gameObject.GetComponent<RectTransform>();
		if (mine != null && refTexture != null)
		{
			float ratioTex = refTexture.rect.width * 1.0f / refTexture.rect.height;
			float ratioScreen = Screen.width * 1.0f / Screen.height;
            float finalHeight = ratioScreen < ratioTex ? Screen.height : Screen.width / ratioTex;
            mine.anchoredPosition = mine.anchorMax = mine.anchorMin = Vector2.one * 0.5f;
            mine.sizeDelta = new Vector2(finalHeight * ratioTex, finalHeight);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
