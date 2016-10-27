using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : Manager
{
	#region Public Fields

	public BoomonRole BoomonRole
	{
		get { return _boomonRole; }
		set
		{
			if (value == _boomonRole)
				return;
			_boomonRole = value;
			RespawnBoomon(value);

#if UNITY_EDITOR
			_boomonRoleEditor = value;
#endif
		}
	}
	#endregion 


	//=====================================================================


	#region Public Methods

	public BoomonController Player { get; private set; }

	public void StartRoom(string roomName)
	{
		Player = null;
		StartCoroutine(LoadSceneCoroutine(roomName));
	}


	public void RespawnBoomon(BoomonRole boomonRole)
	{
		GameObject spawnPoint = Player != null ? Player.gameObject:
				GameObject.FindGameObjectWithTag(_spawnTag);

		if (spawnPoint == null) {
			Debug.LogWarning("GameManager::Spawn>> Spawn point not found");
			return;
		}

		if (Player != null)
			Destroy(Player.gameObject);

		string boomonPath = PathSolver.Instance.GetBoomonPath(boomonRole, 
			PathSolver.InstanceType.Controllable);
		
		GameObject boomonGo = (GameObject)Instantiate(
			Resources.Load<GameObject>(boomonPath),
			spawnPoint.transform.position, spawnPoint.transform.rotation);

		Player = boomonGo.GetComponent<BoomonController>();
	}

	#endregion

	//=======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();
		_boomonRole = _boomonRoleEditor;
	}

	protected void OnDestroy()
	{
		Player = null;
	}

	protected void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	protected void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Update()
	{		 

#if UNITY_EDITOR || UNITY_STANDALONE
		int pressedNumber;
		if( int.TryParse(Input.inputString, out pressedNumber)  
		 && Enum.IsDefined(typeof(BoomonRole), pressedNumber - 1))
			BoomonRole = (BoomonRole) (pressedNumber - 1);
#endif

#if UNITY_EDITOR
		BoomonRole= _boomonRoleEditor;
#endif
	}
	
	#endregion

	//====================================================

	#region Events

	private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
	{
		// HACKing ético FRS -> TODO 161027 Unificar 
		if (!scene.name.Contains("Room"))
			return;

		RespawnBoomon(BoomonRole);
		Transition.Instance.StartAnim(1f, true); // TODO FRS 161027 Configurar un tiempo por defecto de transición
	}

	#endregion
						   
	//========================================================

	#region Private Methods

	private IEnumerator LoadSceneCoroutine(string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
		if (operation == null)
			yield break;

		LoadScreen.Instance.Show(sceneName);
		
		yield return new WaitUntil(() => {
			LoadScreen.Instance.ProgressRatio = operation.progress;
			return operation.isDone;
		});

		LoadScreen.Instance.IsVisible = false;
	}

	#endregion
						  
	//========================================================================================
						  
	#region Private Fields

	[SerializeField] private string _spawnTag = "Respawn";
	[SerializeField] private BoomonRole _boomonRoleEditor;

	private BoomonRole _boomonRole;

	#endregion

}
