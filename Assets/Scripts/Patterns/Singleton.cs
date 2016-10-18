using System;
using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	
	public static T Instance
	{
		get { return GetInstance<T>(); }
	}

	public static TU GetInstance<TU>() where TU : T
	{
		if (_isApplicationQuitting)
		{
			Debug.LogWarning("[Singleton] Instance '" + typeof(TU) +
							 "' already destroyed on application quit." +
							 " Won't create again - returning null.");
			return null;
		}

		lock (_lock)
		{
			if (_instance == null)
			{
				TU[] instances = FindObjectsOfType<TU>();

				Debug.Assert(instances.Length <= 1,
					"[Singleton] Something went really wrong " +
					" - there should never be more than 1 singleton!" +
					" Reopening the scene might fix it.");

				if (instances.Length > 0)
				{
					_instance = instances[0];
					Debug.Log("[Singleton] Using instance already created: " +
							  _instance.gameObject.name);
				}
				else
				{
					GameObject singleton = new GameObject();
					_instance = singleton.AddComponent<TU>();
					singleton.name = typeof(TU).ToString();

					DontDestroyOnLoad(singleton);

					Debug.Log("[Singleton] An instance of " + typeof(TU) +
							  " is needed in the scene, so '" + singleton +
							  "' was created with DontDestroyOnLoad.");
				}
			}

			return (TU) _instance;
		}
	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else if(this != _instance)
		{
			Destroy(gameObject);
		}

	}

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	/// 
	protected virtual void OnDestroy()
	{
		if (this != _instance)
		{
			Debug.Log(name + "::OnDestroy>> Duplicated singleton");
		}
		else
		{
			Debug.Log(name + "::OnDestroy>> Application quit");
			_instance = null;
			_isApplicationQuitting = true;
		}
	}

	//====================================================

	#region Private Methods
	#endregion

	//=======================================================

	#region Private Fields
	private static T _instance;
	private static bool _isApplicationQuitting;
	private static object _lock = new object();
	#endregion
}