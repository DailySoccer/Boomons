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
		}
	}
	#endregion 


	//=====================================================================


	#region Public Methods

	public BoomonController Boomon { get; private set; }

	public void LoadScene(string sceneName)
	{
		Boomon = null;
		StartCoroutine(LoadSceneCoroutine(sceneName));
	}


	public void RespawnBoomon(BoomonRole boomonRole)
	{
		GameObject spawnPoint = Boomon != null ? Boomon.gameObject:
				GameObject.FindGameObjectWithTag(_spawnTag);

		if (spawnPoint == null) {
			Debug.LogWarning("GameManager::Spawn>> Spawn point not found");
			return;
		}

		if (Boomon != null)
			Destroy(Boomon.gameObject);

		string boomonPath = PathSolver.Instance.GetBoomonPath(boomonRole, 
			PathSolver.InstanceType.Controllable);
		
		GameObject boomonGo = (GameObject)Instantiate(
			Resources.Load<GameObject>(boomonPath),
			spawnPoint.transform.position, spawnPoint.transform.rotation);

		Boomon = boomonGo.GetComponent<BoomonController>();
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
		Boomon = null;
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
		if (Input.GetKey(KeyCode.Escape))
			OnEscape();

#if UNITY_EDITOR
		BoomonRole = _boomonRoleEditor;
#endif
	}
	
	#endregion

	//====================================================

	#region Events

	private void OnEscape()
	{
		if(SceneManager.GetActiveScene().name == _mainMenuSceneName)
			Application.Quit();
		else
			LoadScene(_mainMenuSceneName);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
	{
		if (scene.name == _mainMenuSceneName)
			return;

		RespawnBoomon(BoomonRole);
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

	[SerializeField] private string _mainMenuSceneName = "MainMenu";
	[SerializeField] private string _spawnTag = "Respawn";
	[SerializeField] private BoomonRole _boomonRoleEditor;

	private BoomonRole _boomonRole;

	#endregion

}
