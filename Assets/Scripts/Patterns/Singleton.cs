using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour 
	where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			//if (_isApplicationQuitting)
			//{
			//	Debug.LogWarning("Singleton::Instance>> '" + typeof(T) +
			//					 "' already destroyed on application quit." +
			//					 " Won't create again - returning null.");
			//	return null;
			//}

			return _instance ?? (_instance = FindInstance() ?? AutoInstantiate());
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
			DontDestroyOnLoad(this);
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

	

	private static T FindInstance()
	{
		T[] instances = FindObjectsOfType<T>();

		Debug.Assert(instances.Length <= 1,
			"[Singleton] Something went really wrong " +
			" - there should never be more than 1 singleton!" +
			" Reopening the scene might fix it.");

		if (instances.Length > 0) {
			Debug.Log("[Singleton] Using instance already created: " +
						instances[0].gameObject.name);
			return _instance = instances[0];

		} else {
			return null;
		}
	}


	private static T AutoInstantiate()
	{
		string instanceName = typeof (T).Name;

		string prefabPath = string.Format(SingletonPathFormat, instanceName);
		var prefab = Resources.Load<GameObject>(prefabPath);

		T instance = prefab != null 
			? Instantiate(prefab).GetComponent<T>()
			: new GameObject().AddComponent<T>();
			
		instance.name = instanceName;
		DontDestroyOnLoad(instance);

		Debug.Log("Singleton::AutoInstantiate>> An instance of " + instanceName +
				" is needed in the scene, so it was created with DontDestroyOnLoad.");

		return instance;
	}



	#endregion

	//=======================================================

	#region Private Fields

	private const string SingletonPathFormat = "Singletons/{0}";

	private static T _instance;
	private static bool _isApplicationQuitting;
	#endregion
}