﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RotationTrack : MonoBehaviour {

	public Transform TargetCamera = null;
	public RectTransform TargetCanvasPanel = null;
	public AudioClip TargetClip;
	public MeshRenderer[] DisableMeshesList;
	[SerializeField]
	private List<GameObject> DisableGameObList = new List<GameObject>();
	[SerializeField]
	private List<GameObject> EnableGameObList = new List<GameObject>();
	public RectTransform DisableCanvasPanel;
	public Item DisableTelescope;
	public AudioSource DisableMusic;
	[Range(0, 1)]
	public float VolumeAttenuance;
	public Camera SceneCamera;
	// Use this for initialization
	void Awake () {
		_initialized = TargetCamera != null && TargetCanvasPanel != null;
		if (!_initialized)
		{
			Debug.LogError("<color=orange> Instance of type " + this.GetType() + " not initialized." + (TargetCamera == null ? " Reference to TargetCamera missing." : string.Empty));
		}
		else
		{
			if (TargetClip != null)
			{
				_auS = gameObject.AddComponent<AudioSource>();
				_auS.playOnAwake = true;
				_auS.clip = TargetClip;
				_auS.loop = true;
			}
			
			_active = true;
			this.enabled = false;
			_SceneCameraRelated = SceneCamera.gameObject.GetComponent<AudioListener>();
		}
	}


	
	// Update is called once per frame
	void Update () {
        if (_initialized && _active)
        {
            if (Input.gyro != null && Input.gyro.enabled)
            {
                Vector3 gyroRot = -Input.gyro.rotationRate;
                gyroRot.z = -gyroRot.z;
                TargetCamera.transform.Rotate(gyroRot);
            }
            else
            {
                //TODO handle screen controls to rotate camera
            }
		}
	}

	private void OnEnable()
	{
		if (_active)
			return;

		GameObject[] playerTag = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject go in playerTag) {
			if(!DisableGameObList.Contains(go)) {
				DisableGameObList.Add(go);
			}
		}

		GameObject[] InactiveTag = GameObject.FindGameObjectsWithTag("InactiveOnTelescope");
		foreach(GameObject go in InactiveTag) {
			if(!DisableGameObList.Contains(go)) {
				DisableGameObList.Add(go);
			}
		}
		GameObject[] ActiveTag = GameObject.FindGameObjectsWithTag("ActiveOnTelescope");
		foreach(GameObject go in ActiveTag) {
			if(!EnableGameObList.Contains(go)) {
				EnableGameObList.Add(go);
			}
		}

		SetActive(true);
	}

	void OnDisable()
	{
		if (_active)
		{
			SetActive(false);
		}
	}

	private void SetActive(bool active)
	{
		_active = active;
		TargetCamera.gameObject.SetActive(_active);
		if (TargetCanvasPanel != null)
		{
			TargetCanvasPanel.gameObject.SetActive(_active);
		}
		if (_auS != null)
		{
			_auS.enabled = active;
		}
		foreach (GameObject go in EnableGameObList)
		{
			go.SetActive(_active);
		}
		foreach (MeshRenderer mr in DisableMeshesList)
		{
			mr.enabled = !_active;
		}
		foreach (GameObject go in DisableGameObList)
		{
			if (go != null)
			{
				go.SetActive(!_active);
			}
		}
		if (DisableCanvasPanel != null)
		{
			DisableCanvasPanel.gameObject.SetActive(!_active);
		}
		if (DisableMusic != null)
		{
			DisableMusic.volume = active ? VolumeAttenuance : 1;
		}
		if (SceneCamera != null)
		{
			SceneCamera.enabled = !_active;
		}
		DisableTelescope.enabled = !_active;
		if (_SceneCameraRelated != null)
		{
			_SceneCameraRelated.enabled = !_active;
		}
		Input.gyro.enabled = _active;
		Vector3 accel = Input.acceleration;
		Vector3 upRelDir = new Vector3 (-accel.x , -accel.y, accel.z);
		Quaternion rotFix = Quaternion.FromToRotation(upRelDir, Vector3.up);
		TargetCamera.rotation = rotFix;
	}

	private bool _initialized;
	private bool _active;

	private AudioSource _auS;
	private AudioListener _SceneCameraRelated;
}
