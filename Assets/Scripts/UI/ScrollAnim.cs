using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Scrollbar))]
public class ScrollAnim : MonoBehaviour {

    [Range(1,200)]
    public float LoopTime = 30;
	// Use this for initialization
	void Start () {
        _me = GetComponent<Scrollbar>();
        _me.value = 1;
        _startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        _me.value = 1 - (((Time.time - _startTime) % LoopTime) / LoopTime);
	}

    private Scrollbar _me;
    private float _startTime;
}
