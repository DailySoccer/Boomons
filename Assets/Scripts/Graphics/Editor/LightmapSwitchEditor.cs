
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightmapSwitch))]
[CanEditMultipleObjects]
public class LightmapSwitchEditor : Editor
{

	private void OnEnable()
	{
		_periodNames = Enum.GetNames(typeof(LightmapSwitch.DayPeriod));

		_period = serializedObject.FindProperty("_period");
		_isThereChange = serializedObject.FindProperty("_isThereChange");
		_period = serializedObject.FindProperty("_period");
		DayNear = serializedObject.FindProperty("DayNear");
		DayFar = serializedObject.FindProperty("DayFar");
		NightNear = serializedObject.FindProperty("NightNear");
		NightFar = serializedObject.FindProperty("NightFar");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.PropertyField(DayNear, true);
		EditorGUILayout.PropertyField(DayFar, true);
		EditorGUILayout.PropertyField(NightNear, true);
		EditorGUILayout.PropertyField(NightFar, true);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Day Period");
		int oldIndex = Mathf.FloorToInt(_period.floatValue);
		int indexSelected = EditorGUILayout.Popup(oldIndex, _periodNames);
		_isThereChange.boolValue = oldIndex != indexSelected;
		_period.floatValue = indexSelected;
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	private SerializedProperty _period;
	private SerializedProperty _isThereChange;
	private SerializedProperty DayNear;
	private SerializedProperty DayFar;
	private SerializedProperty NightNear;
	private SerializedProperty NightFar;
	private string[] _periodNames;
}
