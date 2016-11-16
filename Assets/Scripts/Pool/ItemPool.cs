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
			DirectoryInfo metaFolder = new DirectoryInfo(Application.dataPath + FolderSeparator + directoryLocation);
			FileInfo[] fileList = metaFolder.GetFiles();
			foreach (FileInfo fileInf in fileList)
			{
				string prefabName = fileInf.FullName;
				if (prefabName.Contains(".prefab") && !prefabName.Contains(".meta"))
				{
					int lowLimit = prefabName.IndexOf("Resources") + 10;
					int highLimit = prefabName.IndexOf(".prefab");
					prefabs.Add(Resources.Load<GameObject>(prefabName.Substring(lowLimit, highLimit - lowLimit)));
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private List<GameObject> prefabs = new List<GameObject>();
#if UNITY_EDITOR_WIN
	private const string FolderSeparator = "\\";
#else
	private const string FolderSeparator = "/";
#endif
}
