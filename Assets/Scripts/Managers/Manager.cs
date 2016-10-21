
using UnityEngine;


public abstract class Manager : MonoBehaviour
{
	/// <summary>
	/// 
	/// </summary>
	protected virtual void Awake()
	{
		MetaManager = MetaManager.Instance;
		MetaManager.Register(this);
	}

	//protected virtual void OnDestroy()
	//{
	//	if (this != _instance)
	//	{
	//		Debug.Log(name + "::OnDestroy>> Duplicated singleton");
	//	}
	//	else
	//	{
	//		Debug.Log(name + "::OnDestroy>> Application quit");
	//		_instance = null;
	//		_isApplicationQuitting = true;
	//	}
	//}

	protected MetaManager MetaManager { get; private set; }
}

