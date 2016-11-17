using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ItemPool : MonoBehaviour {

	public string directoryLocation;
	// Use this for initialization
	void Start () {
		if (!string.IsNullOrEmpty(directoryLocation))
		{
			//Read locations available
			Transform[] childrenTrans = transform.GetComponentsInChildren<Transform>();
			foreach (Transform tf in childrenTrans)
			{
				if (tf != transform)
				{
					_locations.Add(tf);
				}
			}
			//Load prefabs (as much as locations available)
			Object[] prefabList = Resources.LoadAll(directoryLocation, typeof(GameObject));
			foreach (Object obj in prefabList)
			{
				if (_prefabs.Count < _locations.Count)
				{
					GameObject go = obj as GameObject;
					if (go != null)
					{
						_prefabs.Add(go);
					}
				}
			}
			//Generate prefabs on available locations
			for (int i = 0; i < _prefabs.Count; ++i)
			{
				int index = Random.Range(0,_locations.Count);
				Transform nextParent = _locations[index];
				_locations.RemoveAt(index);
				GameObject newGenerated = GameObject.Instantiate<GameObject>(_prefabs[i]);
				newGenerated.transform.SetParent(nextParent);
				newGenerated.transform.localPosition = Vector3.zero;
				newGenerated.transform.localRotation = Quaternion.identity;
			}
		}
	}

	private List<GameObject> _prefabs = new List<GameObject>();
	private List<Transform> _locations = new List<Transform>();
}
