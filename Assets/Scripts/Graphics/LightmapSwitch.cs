using UnityEngine;
using System.Collections;
using System.Linq;

public class LightmapSwitch : MonoBehaviour {

	public enum DayPeriod
	{
		Day = 0,
		Night = 1
	}
	
	[SerializeField]
	private Texture2D[] DayNear;
	[SerializeField]
	private Texture2D[] DayFar;
	[SerializeField]
	private Texture2D[] NightNear;
	[SerializeField]
	private Texture2D[] NightFar;

	public DayPeriod Period
	{
		get { return (DayPeriod)_period; }
		set
		{
			_isThereChange = _period != (float)value;
			_period = (float)value;
		}
	}
	private LightmapData[] DayLightmap;
	private LightmapData[] NightLightmap;
	// Use this for initialization
	void Start () {
		_initialized = DayNear != null && DayFar != null && NightNear != null && NightFar != null;
		if (_initialized)
		{
			/*DayLightmap = new LightmapData[1];
			DayLightmap[0] = new LightmapData();
			DayLightmap[0].lightmapNear = Day[0];
			DayLightmap[0].lightmapFar = Day[1];

			NightLightmap = new LightmapData[1];
			NightLightmap[0] = new LightmapData();
			NightLightmap[0].lightmapNear = Night[0];
			NightLightmap[0].lightmapFar = Night[1];*/

			DayLightmap = new LightmapData[DayNear.Length];
			for (int i = 0; i < DayNear.Length; i++)
			{
				DayLightmap[i] = new LightmapData();
				DayLightmap[i].lightmapNear = DayNear[i];
				DayLightmap[i].lightmapFar = DayFar[i];
			}

			NightLightmap = new LightmapData[NightNear.Length];
			for (int i = 0; i < NightNear.Length; i++)
			{
				NightLightmap[i] = new LightmapData();
				NightLightmap[i].lightmapNear = NightNear[i];
				NightLightmap[i].lightmapFar = NightFar[i];
			}
			_isThereChange = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_initialized && _isThereChange)
		{
			if (Period == DayPeriod.Day)
			{
				LightmapSettings.lightmaps = DayLightmap;
				Debug.Log("Switch to Day");
			}
			else if(Period == DayPeriod.Night)
			{
				LightmapSettings.lightmaps = NightLightmap;
				Debug.Log("Switch to Night");
			}
			_isThereChange = false;
		}
	}

	private bool _initialized;
	[SerializeField]
	private float _period;
	[SerializeField]
	private bool _isThereChange;
}
