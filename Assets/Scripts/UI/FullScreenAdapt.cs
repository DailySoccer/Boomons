using UnityEngine;
using System.Collections;

public class FullScreenAdapt : MonoBehaviour {

	public Camera SceneCamera;
	// Use this for initialization
	void Start () {
		float distanceToCamera = Vector3.Dot(transform.position - SceneCamera.transform.position, SceneCamera.transform.forward);
		float aspectRatio = Screen.width * 1f / Screen.height;
		float relationalAngle = SceneCamera.fieldOfView * 0.5f;
		float heightAtDistance = 2 * distanceToCamera * Mathf.Tan(relationalAngle * Mathf.Deg2Rad);
		transform.localScale = new Vector3(aspectRatio * heightAtDistance, heightAtDistance, 1);
		transform.position = new Vector3(SceneCamera.transform.position.x, SceneCamera.transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
