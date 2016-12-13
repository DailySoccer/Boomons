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
				MetaManager.Instance.Get<GameManager>().BoomonRole = TextToBoomon(data).Value;
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

	public static BoomonRole? TextToBoomon(string boomon)
	{
        /*Debug.Log(boomon);
        Debug.Log("Is equal 'Boomons-Tras-Musico' to '" + boomon + "': "+ ("Boomons-Tras-Musico" == boomon));
        Debug.Log("Is equal 'Boomons-Tras-Musico' to '" + boomon + "': " + ("Boomons-Tras-Musico".Equals(boomon)));
        Debug.Log("Is equal 'Boomons-Tras-Musico' to '" + boomon + "': " + string.Equals("Boomons-Tras-Musico",boomon));*/
        boomon = boomon.Replace("\n", string.Empty).Replace("\r", string.Empty);
        switch (boomon)
		{
			case "Boomons-Mimi-Artista":
				return BoomonRole.Artist;
			case "Boomons-Auri-Naturaleza":
				return BoomonRole.Naturalist;
			case "Boomons-Tati-DeportistaChica":
				return BoomonRole.FemaleSport;
			case "Boomons-Tato-DeportistaChico":
				return BoomonRole.MaleSport;
			case "Boomons-Tras-Musico":
				return BoomonRole.Music;
			case "Boomons-Mike-Manitas":
				return BoomonRole.Maker;
			case "Boomons-Fleki-Gamer":
				return BoomonRole.Gamer;
			default:
				return null;
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
