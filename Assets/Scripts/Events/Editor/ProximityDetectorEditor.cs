using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProximityDetector))]
public class ProximityDetectorEditor : Editor
{
	private ProximityDetector _target;
	private SerializedProperty _proximityRadius;

	private void OnEnable()
	{
		_target = target as ProximityDetector;
		_proximityRadius = serializedObject.FindProperty("_proximityRadius");
	}

   

}

