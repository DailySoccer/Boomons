/*
 * Copyright (C) 2012 GREE, Inc.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WebViewer : MonoBehaviour
{
	public string Url = "http://www.unusualwonder.com/";
	public RectTransform ReferenceScreen;
	WebViewObject webViewObject;

	IEnumerator Start()
	{
		int topMargin = 0, leftMargin = 0, rightMargin = 0, bottomMargin = 0;
		topMargin = Mathf.RoundToInt(Screen.height * (1 - ReferenceScreen.anchorMax.y));
		bottomMargin = Mathf.RoundToInt(Screen.height * ReferenceScreen.anchorMin.y);
		rightMargin = Mathf.RoundToInt(Screen.width * (1 - ReferenceScreen.anchorMax.x));
		leftMargin = Mathf.RoundToInt(Screen.width * ReferenceScreen.anchorMin.x);
		webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
		webViewObject.Init(
			 cb: (msg) =>
			 {
				 Debug.Log(string.Format("CallFromJS[{0}]", msg));
			 },
			 err: (msg) =>
			 {
				 Debug.Log(string.Format("CallOnError[{0}]", msg));
			 },
			 ld: (msg) =>
			 {
				webViewObject.SetVisibility(true);
				Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
#if !UNITY_ANDROID
                webViewObject.EvaluateJS(@"
                  window.Unity = {
                    call: function(msg) {
                      var iframe = document.createElement('IFRAME');
                      iframe.setAttribute('src', 'unity:' + msg);
                      document.documentElement.appendChild(iframe);
                      iframe.parentNode.removeChild(iframe);
                      iframe = null;
                    }
                  }
                ");
#endif
				},
		enableWKWebView: true);
		webViewObject.SetMargins(leftMargin, topMargin, rightMargin, bottomMargin);

#if !UNITY_WEBPLAYER
		if (Url.StartsWith("http"))
		{
			webViewObject.LoadURL(Url.Replace(" ", "%20"));
		}
		else
		{
			var exts = new string[]{
					 ".jpg",
					 ".html"  // should be last
            };
			foreach (var ext in exts)
			{
				var url = Url.Replace(".html", ext);
				var src = System.IO.Path.Combine(Application.streamingAssetsPath, url);
				var dst = System.IO.Path.Combine(Application.persistentDataPath, url);
				byte[] result = null;
				if (src.Contains("://"))
				{  // for Android
					var www = new WWW(src);
					yield return www;
					result = www.bytes;
				}
				else
				{
					result = System.IO.File.ReadAllBytes(src);
				}
				System.IO.File.WriteAllBytes(dst, result);
				if (ext == ".html")
				{
					webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
					break;
				}
			}
		}
#else
        if (Url.StartsWith("http")) {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        } else {
            webViewObject.LoadURL("StreamingAssets/" + Url.Replace(" ", "%20"));
        }
        webViewObject.EvaluateJS(
            "parent.$(function() {" +
            "   window.Unity = {" +
            "       call:function(msg) {" +
            "           parent.unityWebView.sendMessage('WebViewObject', msg)" +
            "       }" +
            "   };" +
            "});");
#endif
		yield break;
	}

#if !UNITY_WEBPLAYER
	/*void OnGUI()
	{
		GUI.enabled = webViewObject.CanGoBack();
		if (GUI.Button(new Rect(10, 10, 80, 80), "<"))
		{
			webViewObject.GoBack();
		}
		GUI.enabled = true;

		GUI.enabled = webViewObject.CanGoForward();
		if (GUI.Button(new Rect(100, 10, 80, 80), ">"))
		{
			webViewObject.GoForward();
		}
		GUI.enabled = true;
	}*/
#endif
}
