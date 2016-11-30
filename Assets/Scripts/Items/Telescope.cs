using UnityEngine;
using System.Collections;

public class Telescope : Item
{
	//=========================================================

	#region Public Methods

	public override void Play()
	{
		Debug.Log("Telescope::Play>> " + name, this);
		_tracker.enabled = true;
	}

	#endregion

	//=========================================================

	#region Mono
	
	protected override void Awake()
	{
		base.Awake();
		Debug.Assert(_tracker != null, "Telescope::Awake>> Tracker not defined!!", this);
	}

	protected override void OnDestroy()
	{
		_tracker = null;
		base.OnDestroy();
	}

	#endregion

	//=============================================================


	#region Private fields
	[SerializeField] private RotationTrack _tracker;
	#endregion 
}
