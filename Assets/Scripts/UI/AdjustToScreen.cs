﻿using UnityEngine;
using System.Collections;

public class AdjustToScreen : MonoBehaviour {

	public Sprite refTexture;
	// Use this for initialization
	void Start () {
		RectTransform mine = gameObject.GetComponent<RectTransform>();
		if (mine != null && refTexture != null)
		{
			float ratioTex = refTexture.texture.width * 1.0f / refTexture.texture.height;
			float ratioScreen = Screen.width * 1.0f / Screen.height;
			float finalHeight = ratioScreen > ratioTex ? Screen.height : Screen.width / ratioTex;
			mine.sizeDelta = new Vector2(finalHeight, finalHeight*ratioTex);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
