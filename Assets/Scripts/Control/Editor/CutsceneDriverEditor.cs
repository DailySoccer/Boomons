
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CutsceneDriver), true)]
[CanEditMultipleObjects]
public class CutsceneDriverEditor : Editor
{

	private void OnEnable()
	{
		_stateNames   = Enum.GetNames(typeof(BoomonController.State));
		_emotionNames = Enum.GetNames(typeof(BoomonController.Emotion));

		_boomonActive = serializedObject.FindProperty("_animableBoomonActive");
		_boomonState  = serializedObject.FindProperty("_animableBoomonState");
		_boomonEmotion = serializedObject.FindProperty("_animableBoomonEmotion");
	}



	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(_boomonActive, new GUIContent("Is Boomon Active"));

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Boomon State");
		_boomonState.floatValue   = EditorGUILayout.Popup( Mathf.FloorToInt(_boomonState.floatValue), _stateNames);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Boomon Emotion");
		_boomonEmotion.floatValue = EditorGUILayout.Popup( Mathf.FloorToInt(_boomonEmotion.floatValue), _emotionNames);
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}


	//public void OnSceneGUI()
	//{
	//	var t = (target as CutsceneDriver);

	//	EditorGUI.BeginChangeCheck();
	//	Vector3 pos = Handles.PositionHandle(t.State, Quaternion.identity);
	//	if(EditorGUI.EndChangeCheck()) {
	//		Undo.RecordObject(target, "Move point");
	//		t.lookAtPoint = pos;
	//		t.Update();
	//	}
	//}


	private SerializedProperty _boomonState;
	private SerializedProperty _boomonEmotion;
	private SerializedProperty _boomonActive;

	private string[] _emotionNames;
	private string[] _stateNames;
	
}
