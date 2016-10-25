using System;
using System.Collections.Generic;
using UnityEngine;


public class MetaManager : Singleton<MetaManager>
{
	public T GetManager<T>()
		where T : Manager
	{
		return FindManager<T>() ?? CreateInstance<T>();
	}


	public void Register(Manager manager)
	{
		Type baseType = FindBaseType(manager.GetType());
		Manager found = FindManager(baseType);

		if(found != null && found != manager) {
			Destroy(manager.gameObject);
			Debug.Log("<b>MetaManager::Register>></b> Destroying duplicated manager " + baseType.Name + ": " + manager.name);

		} else {
			_managers[baseType] = manager;
			DontDestroyOnLoad(manager);
			Debug.Log("<b>MetaManager::Register>></b> " + baseType.Name + " successfully registered", manager);
		}
	}


	//==================================================================

	#region Mono

	
	protected override void OnDestroy()
	{
		_managers.Clear();
		base.OnDestroy();
	}

	#endregion


	//====================================================

	#region Private Methods

	private T FindManager<T>()
		where T : Manager
	{
		return FindManager(typeof (T)) as T;
	}

	private Manager FindManager(Type type)
	{
		Type baseType = FindBaseType(type);

		Manager manager;
		return _managers.TryGetValue(baseType, out manager) ? manager : 
			(_managers[baseType] = FindInstance(baseType));
	}

	private static Type FindBaseType<T>()
		where T : Manager
	{
		return FindBaseType(typeof (T));
	}
	
	private static Type FindBaseType(Type type)
	{
		while (type != null && type.BaseType != _managerType)
			type = type.BaseType;
		return type;
	}

	private Manager FindInstance(Type type)
	{
		UnityEngine.Object[] instances = FindObjectsOfType(type);

		Debug.Assert(instances.Length <= 1,
			"[MetaManager] Something went really wrong " +
			" - there should never be more than 1 singleton!" +
			" Reopening the scene might fix it.");

		if (instances.Length > 0) {
			Debug.Log("<b>MetaManager::FindInstance>></b> Found instance: " + instances[0].name, instances[0]);
			return (Manager) instances[0];

		} else {
			return null;
		}
	}


	private static T CreateInstance<T>()
		where T : Manager
	{
		string instanceName = typeof (T).Name;

		var prefab = Resources.Load<GameObject>(PathSolver.Instance.GetManagerPath<T>());

		Debug.Log("<b>MetaManager::CreateInstance>></b> An instance of " + instanceName +
				" is needed and being created.");

		T instance = prefab != null 
			? Instantiate(prefab).GetComponent<T>()
			: new GameObject().AddComponent<T>();
			
		instance.name = instanceName;
		
		return instance;
	}



	#endregion

	//=======================================================

	#region Private Fields

	private static readonly Type _managerType = typeof(Manager);
	private readonly Dictionary<Type, Manager> _managers = new Dictionary<Type, Manager>();

	#endregion


}