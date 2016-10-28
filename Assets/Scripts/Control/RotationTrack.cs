using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotationTrack : MonoBehaviour {

	public Transform TargetCamera = null;
	public RectTransform TargetCanvasPanel = null;
	public AudioClip TargetClip;
	public MeshRenderer[] DisableMeshesList;
	public RectTransform DisableCanvasPanel;
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
				auS = gameObject.AddComponent<AudioSource>();
				auS.playOnAwake = true;
				auS.clip = TargetClip;
				auS.loop = true;
			}
			_currentRot = Quaternion.identity;
			_active = true;
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_initialized && _active)
		{
			Vector3 gyroRot = -Input.gyro.rotationRate;
			gyroRot.z = -gyroRot.z;
			TargetCamera.transform.Rotate(gyroRot);
		}
	}

	void OnEnable()
	{
		if (!_active)
		{
			SetActive(true);
		}
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
		TargetCanvasPanel.gameObject.SetActive(_active);
		if (auS != null)
		{
			auS.enabled = active;
		}
		foreach (MeshRenderer mr in DisableMeshesList)
		{
			mr.enabled = !_active;
		}
		if (DisableCanvasPanel != null)
		{
			DisableCanvasPanel.gameObject.SetActive(!_active);
		}
		if (DisableMusic != null)
		{
			DisableMusic.volume = active ? VolumeAttenuance : 1;
		}
		SceneCamera.enabled = !_active;
		Input.gyro.enabled = _active;
		Vector3 accel = Input.acceleration;
		Vector3 upRelDir = new Vector3 (-accel.x , -accel.y, accel.z);
		Quaternion rotFix = Quaternion.FromToRotation(upRelDir, Vector3.up);
		TargetCamera.rotation = rotFix;
	}

	private bool _initialized;
	private bool _active;
	private Quaternion _currentRot;
	private AudioSource auS;
}
