using UnityEngine;
using System.Collections;

using ZXing.QrCode;
using ZXing;
using ZXing.Common;

public class WebcamToTexture : MonoBehaviour {

	public Renderer CameraView;
	public ResultQR Listener;
	// Use this for initialization
	void Awake () {
		_initialized = CameraView != null && Listener != null;
		if (_initialized)
		{
			if (_cameraFeed == null)
			{
				_cameraFeed = new WebCamTexture();
			}
			/*_cameraFeed.requestedWidth = Screen.width;
			_cameraFeed.requestedHeight = Screen.height;
			_cameraFeed.Play();*/
			//_baseRotation = transform.rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_initialized && _cameraFeed != null && _cameraFeed.isPlaying)
		{
			CheckUpdate();
			//transform.rotation = _baseRotation * Quaternion.AngleAxis(_cameraFeed.videoRotationAngle, Vector3.up);
			if (_processQR)
			{
				_camTexture = _cameraFeed.GetPixels32();
				ProcessQR();
			}
		}
	}

	void OnEnable()
	{
		if (_cameraFeed != null)
		{
			_cameraFeed.requestedWidth = Mathf.FloorToInt(Screen.width * WebcamResolution);
			_cameraFeed.requestedHeight = Mathf.FloorToInt(Screen.height * WebcamResolution);

#if !QR_FAKER
			_cameraFeed.Play();
#endif
			CameraView.sharedMaterial.mainTexture = _cameraFeed;
			CameraView.transform.RotateAround(CameraView.transform.position, CameraView.transform.forward, -_cameraFeed.videoRotationAngle);
			_timeCounter = 0;
		}
	}

	void OnDisable()
	{
		_cameraFeed.Stop();
	}

	private void ProcessQR()
	{
		if (_qrReader == null)
		{
			_qrReader = new BarcodeReader();
		}

#if QR_FAKER
		Listener.SetResult(new System.Random().NextDouble() > .1, "Boomons-Auri-Naturaleza");
#else
		Result QRRes = _qrReader.Decode(_camTexture, _cameraFeed.width, _cameraFeed.height); //_qrReader.decode(new BinaryBitmap(new HybridBinarizer(new RGBLuminanceSource(_rawCamTex, W, H))));
		if (QRRes != null && Listener != null)
		{
			string decodedString = QRRes.Text;
			Listener.SetResult(ResultQR.TextToBoomon(decodedString).HasValue, decodedString);
		}
#endif
	}

	private void CheckUpdate()
	{
		_timeCounter += Time.deltaTime;
		_processQR = _timeCounter > _TIME_STEP;
		_timeCounter %= _TIME_STEP;
	}

	private WebCamTexture _cameraFeed;
	//private Quaternion _baseRotation;
	private bool _initialized;
	private bool _processQR;
	private Color32[] _camTexture;
	private byte[] _rawCamTex;
	private BarcodeReader _qrReader;
	private string _decodedResult;
	private float _timeCounter;
	private float _TIME_STEP = 2;
	private float WebcamResolution = 0.6f;

	}
