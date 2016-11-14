using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultQR : MonoBehaviour {

	public RectTransform Background;
	public RectTransform ScanningPanel;
	public RectTransform CorrectPanel;
	public RectTransform IncorrectPanel;
	public bool Result;
	// Use this for initialization
	void Awake () {
		_initialized = CorrectPanel != null && IncorrectPanel != null && ScanningPanel != null && Background != null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetResult(bool result, string data)
	{
		if (_initialized)
		{
			ScanningPanel.gameObject.SetActive(false);
			Background.gameObject.SetActive(true);
			Result = result;
			if (result)
			{
				switch (data)
				{
					default:
						MetaManager.Instance.GetManager<GameManager>().BoomonRole = BoomonRole.Music;
						break;
				}
				PlayerPrefs.SetString("Room 1", "");
				PlayerPrefs.SetString("Room 2", "");
				PlayerPrefs.SetString("Room 3", "");
				PlayerPrefs.SetString("Room 4", "");
				PlayerPrefs.Save();
			}
			CorrectPanel.gameObject.SetActive(Result);
			IncorrectPanel.gameObject.SetActive(!Result);
		}
	}

	public void StartScan()
	{
		if (_initialized)
		{
			ScanningPanel.gameObject.SetActive(true);
			Background.gameObject.SetActive(false);
			CorrectPanel.gameObject.SetActive(false);
			IncorrectPanel.gameObject.SetActive(false);
		}
	}

	private bool _initialized;
}
