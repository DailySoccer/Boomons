using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CheckConnection : MonoBehaviour {
	
	public RectTransform ConnectionPanel;
	public RectTransform DisconnectionPanel;
	void Start()
	{
		ConnectionPanel.gameObject.SetActive(Application.internetReachability != NetworkReachability.NotReachable);
		DisconnectionPanel.gameObject.SetActive(Application.internetReachability == NetworkReachability.NotReachable);
	}
	// Use this for initialization
	//void Start ()
	//{
	//	WWW www = new WWW(URL);
	//	//yield return www;
	//	ConnectionPanel.gameObject.SetActive(www.error != null);
	//	DisconnectionPanel.gameObject.SetActive(www.error != null);
	//}

	// Update is called once per frame
	void Update () {
	
	}
}
