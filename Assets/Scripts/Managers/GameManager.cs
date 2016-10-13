

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	#region Public Methods

	public BoomonController Boomon { get; private set; }

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}


	public void RespawnBoomon()
	{
		GameObject spawnPoint = GameObject.FindGameObjectWithTag(_spawnTag);
		Debug.Assert(spawnPoint != null, "GameManager::Spawn>> Spawn point not found");

		GameObject boomonGo = (GameObject)Instantiate(_boomonPrefab,
			spawnPoint.transform.position, spawnPoint.transform.rotation);
		Boomon = boomonGo.GetComponent<BoomonController>();
	}

	#endregion

	//=======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();
		_boomonPrefab = Resources.Load<GameObject>(_boomonPath);
	}

	protected override void OnDestroy()
	{
		Boomon = null;
		_boomonPrefab = null;
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

		RespawnBoomon();
	}

	#endregion


	//========================================================

	#region Private Methods



	#endregion

	//=================================================================
	#region Private Fields

	[SerializeField] private string _mainMenuSceneName = "MainMenu";
	[SerializeField] private string _boomonPath = "Prefabs/Boomon";
	[SerializeField] private string _spawnTag = "Spawn";

	private GameObject _boomonPrefab;

	#endregion

}
