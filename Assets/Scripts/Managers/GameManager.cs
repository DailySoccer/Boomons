using System;
using UnityEngine;
using UnityEngine.SceneManagement;

			
public class GameManager : Manager
{
	#region Public Fields

	public ReferenceSystem ReferenceSystem {
		get { return Boomon.ReferenceSystem;  }
	}
	public BoomonController Boomon { get; private set; }
	public BoomonCamera Camera { get; private set; }


	public BoomonRole BoomonRole
	{
		get { return _boomonRole; }
		set
		{
			if (value == _boomonRole)
				return;
			_boomonRole = value;
#if UNITY_EDITOR
			_boomonRoleEditor = value;
#endif						
			if(value != BoomonRole.None && Boomon != null)
				SpawnBoomon(value, Boomon.transform);
		}
	}


	public string Room { get; set; }
	public string ActiveRoom { get; private set; }
	public bool IsReadyToPlay  {
		get {
			return BoomonRole != BoomonRole.None 
				&& !string.IsNullOrEmpty(Room);
		}
	}

	

	#endregion


	//=====================================================================


	#region Public Methods	

	public void Play()
	{
		Debug.Assert(IsReadyToPlay, "GameManager::Play>> Game not ready yet!!");
		if(IsReadyToPlay)
			SceneLoader.Instance.GoToRoom(Room);
	}

	#endregion

	//=======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();
#if UNITY_EDITOR
		_boomonRole = _boomonRoleEditor;
#endif
	}

	protected void OnDestroy()
	{
		Boomon = null;
		Camera = null;
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
			&& Enum.IsDefined(typeof(BoomonRole), pressedNumber) )
			BoomonRole = (BoomonRole) pressedNumber;
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
		Camera = UnityEngine.Camera.main.GetComponent<BoomonCamera>();
		Debug.Assert(Camera != null, "GameManager::Start>> Camera not found!!", this);

		Transform spawner = FindSpawner();
		
		if(spawner != null) 
			OnRoomEnter(scene.name, spawner);
		else if(ActiveRoom != null)
			OnRoomExit(ActiveRoom);
	}

	private void OnRoomEnter(string roomName, Transform spawner)
	{
		ActiveRoom = roomName;
		Transition.Instance.StartAnim(1f, true); // TODO FRS 161027 Configurar un tiempo por defecto de transición
		SpawnBoomon(BoomonRole, spawner);
	}

	private void OnRoomExit(string roomName)
	{
		ActiveRoom = Room = null;
		BoomonRole = BoomonRole.None;
		Boomon = null;
	}

	#endregion

	//========================================================

	#region Private Methods

	private void SpawnBoomon(BoomonRole boomonRole, Transform spawner)
	{
		if (boomonRole == BoomonRole.None) {
			Debug.LogError("GameManager::SpawnBoomon>> Boomon role not defined!!", this);
			return;
		}

		if(Boomon != null)
			Destroy(Boomon.gameObject);

		string boomonPath = PathSolver.Instance.GetBoomonPath(boomonRole, PathSolver.InstanceType.Controllable);
		var prefab = Resources.Load<GameObject>(boomonPath);
		var boomonGo = Instantiate(prefab, spawner.position, spawner.rotation);

		Boomon = boomonGo.GetComponent<BoomonController>();
		MetaManager.Instance.Get<ObjectTouchManager>().enabled = true;
	}

	private Transform FindSpawner()
	{
		GameObject spawnPoint = GameObject.FindGameObjectWithTag(_spawnTag);

		if(spawnPoint == null) {
			Debug.LogWarning("GameManager::Spawn>> Spawn point not found");
			return null;

		} else {
			return spawnPoint.transform;
		}
	}

	#endregion
						  
	//========================================================================================
						  
	#region Private Fields

	[SerializeField] private string _spawnTag = "Respawn";

#if UNITY_EDITOR   
	[SerializeField] private BoomonRole _boomonRoleEditor;
#endif

	private BoomonRole _boomonRole;

	#endregion

	

}
