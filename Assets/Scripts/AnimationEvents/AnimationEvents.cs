using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

	public delegate void VoidNoParams();

	public event VoidNoParams OnClipStart;
	public event VoidNoParams OnClipEnd;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExecuteOnClipStart()
	{
		if (OnClipStart != null)
		{
			OnClipStart();
		}
	}

	public void ExecuteOnClipEnd()
	{
		if (OnClipEnd != null)
		{
			OnClipEnd();
		}
	}
}
