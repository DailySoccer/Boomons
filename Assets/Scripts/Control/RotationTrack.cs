using UnityEngine;
using System.Collections;

public class RotationTrack : MonoBehaviour {

	public Transform TargetCamera = null;
	// Use this for initialization
	void Start () {
		_initialized = TargetCamera != null;
		if (!_initialized)
		{
			Debug.LogError("<color=orange> Instance of type " + this.GetType() + " not initialized." + (TargetCamera == null ? " Reference to TargetCamera missing." : string.Empty));
		}
		else
		{
			_currentRot = Quaternion.identity;
			_active = false;
			TargetCamera.gameObject.SetActive(false);
			//TODO erase the next line
			Activate();
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

	public void Activate()
	{
		if (!_active)
		{
			SetActive(true);
		}
	}

	public void Deactivate()
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
		Input.gyro.enabled = _active;
		Vector3 accel = Input.acceleration;
		Vector3 upRelDir = new Vector3 (-accel.x , -accel.y, accel.z);
		Quaternion rotFix = Quaternion.FromToRotation(upRelDir, Vector3.up);
		TargetCamera.rotation = rotFix;
	}

	private bool _initialized;
	private bool _active;
	private Quaternion _currentRot;
}
