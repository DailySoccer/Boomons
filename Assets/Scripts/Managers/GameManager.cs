using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BoomonType
{
	Artist = 0,
	Gamer = 1,
	Maker = 2,
	Music = 3,
	Naturalist = 4,
	FemaleSport = 5,
	MaleSport = 6
}


public class GameManager : Singleton<GameManager>
{
	#region Public Fields

	public BoomonType BoomonType
	{
		get { return _boomonType; }
		set
		{
			if (value == _boomonType)
				return;
			_boomonType = value;
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


	public void RespawnBoomon(BoomonType boomonType)
	{
		GameObject spawnPoint = Boomon != null ? Boomon.gameObject:
				GameObject.FindGameObjectWithTag(_spawnTag);

		if (spawnPoint == null) {
			Debug.LogWarning("GameManager::Spawn>> Spawn point not found");
			return;
		}

		if (Boomon != null)
			Destroy(Boomon.gameObject);

		string boomonPath = string.Format(_boomonPathFormat, boomonType);
		
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
		_boomonType = _boomonTypeEditor;
	}

	protected override void OnDestroy()
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
		BoomonType = _boomonTypeEditor;
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

		RespawnBoomon(BoomonType);
	}

	#endregion


	//========================================================

	#region Private Methods

	private IEnumerator LoadSceneCoroutine(string sceneName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
		if (operation == null)
			yield break;

		LoadScreenManager.Instance.Show(sceneName);
		
		yield return new WaitUntil(() => {
			LoadScreenManager.Instance.ProgressRatio = operation.progress;
			return operation.isDone;
		});

		LoadScreenManager.Instance.IsVisible = false;
	}

	#endregion


	//========================================================================================


	#region Private Fields

	[SerializeField] private string _mainMenuSceneName = "MainMenu";
	[SerializeField] private string _spawnTag = "Respawn";
	[SerializeField] private string _boomonPathFormat = "Characters/{0}Boomon";
	[SerializeField] private BoomonType _boomonTypeEditor;

	private BoomonType _boomonType;

	#endregion

}
