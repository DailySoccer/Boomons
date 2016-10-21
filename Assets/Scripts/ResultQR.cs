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
			CorrectPanel.gameObject.SetActive(result);
			IncorrectPanel.gameObject.SetActive(!result);
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
