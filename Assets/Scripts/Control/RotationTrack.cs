using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RotationTrack : MonoBehaviour {

	public Transform TargetCamera = null;
	public RectTransform TargetCanvasPanel = null;
    public TelescopeControls ControlsCanvas = null;
	public AudioClip TargetClip;
	public MeshRenderer[] DisableMeshesList;
	[SerializeField]
	private List<GameObject> DisableGameObList = new List<GameObject>();
	[SerializeField]
	private List<GameObject> EnableGameObList = new List<GameObject>();
	public RectTransform DisableCanvasPanel;
	public Item DisableTelescope;
	public AudioSource DisableMusic;
    public float VerticalRotationSpeed = 15;
    public float HorizontalRotationSpeed = 180;
    [Range(0,89)]
    public float VerticalLimitAngle = 45;
	[Range(0, 1)]
	public float VolumeAttenuance;
	public Camera SceneCamera;
	// Use this for initialization
	void Awake () {
		_initialized = TargetCamera != null && TargetCanvasPanel != null && ControlsCanvas != null;
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
        _hasGyro = SystemInfo.supportsGyroscope;
	}


	
	// Update is called once per frame
	void Update () {
        if (_initialized && _active)
        {
            if (_hasGyro)
            {
                Vector3 gyroRot = -Input.gyro.rotationRate;
                gyroRot.z = -gyroRot.z;
                TargetCamera.transform.Rotate(gyroRot);
            }
            else
            {
                Vector3 rotAngles = TargetCamera.transform.rotation.eulerAngles;
                rotAngles.x -= Time.deltaTime * VerticalRotationSpeed * ControlsCanvas.GetVertical();
                rotAngles.y += Time.deltaTime * HorizontalRotationSpeed * ControlsCanvas.GetHorizontal();
                if(rotAngles.x > 180)
                    rotAngles.x = Mathf.Clamp(rotAngles.x, 360-VerticalLimitAngle, 360+VerticalLimitAngle);
                else
                    rotAngles.x = Mathf.Clamp(rotAngles.x, -VerticalLimitAngle, VerticalLimitAngle);
                TargetCamera.transform.rotation = Quaternion.Euler(rotAngles);
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
        if (_hasGyro)
        {
            Input.gyro.enabled = _active;
            Vector3 accel = Input.acceleration;
            Vector3 upRelDir = new Vector3(-accel.x, -accel.y, accel.z);
            Quaternion rotFix = Quaternion.FromToRotation(upRelDir, Vector3.up);
            TargetCamera.rotation = rotFix;
        }
        else
        {
            ControlsCanvas.gameObject.SetActive(_active);
        }
	}

	private bool _initialized;
	private bool _active;
    private bool _hasGyro;

	private AudioSource _auS;
	private AudioListener _SceneCameraRelated;
}
