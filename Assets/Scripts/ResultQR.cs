using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultQR : MonoBehaviour {

	public RectTransform CorrectPanel;
	public RectTransform IncorrectPanel;
	public RectTransform CorrectBackMenu;
	public RectTransform IncorrectBackMenu;
	public bool Result;
	// Use this for initialization
	void Start () {
		_initialized = CorrectPanel != null && IncorrectPanel != null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetResult(bool result)
	{
		if (_initialized)
		{
			Result = result;
			PlayerPrefs.SetInt("ExtraContents", 1);
			PlayerPrefs.Save();
			CorrectPanel.gameObject.SetActive(Result);
			IncorrectPanel.gameObject.SetActive(!Result);
		}
	}

	public void GoBackToMenu()
	{
		if (_initialized)
		{
			CorrectBackMenu.gameObject.SetActive(Result);
			IncorrectBackMenu.gameObject.SetActive(!Result);
		}
	}

	private bool _initialized;
}
