using UnityEngine;
using System.Collections;

public class TelescopeActivator : MonoBehaviour {

	public RotationTrack TrackerRef;
	// Use this for initialization
	void Start () {
		_initialized = TrackerRef != null;
		_readyToStart = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClipEnd()
	{
		if (_initialized && _readyToStart)
		{
			TrackerRef.enabled = true;
			_readyToStart = false;
			Debug.Log("<color=orange> Clip end</color>");
		}
	}

	public void ClipStart()
	{
		_readyToStart = true;
		Debug.Log("<color=yellow> Clip start</color>");
	}

	public void TurnOff()
	{
		_readyToStart = false;
	}

	private bool _initialized;
	private bool _readyToStart;
}
