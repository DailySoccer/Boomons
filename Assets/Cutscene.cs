using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cutscene : MonoBehaviour
{

	#region Mono

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_driver = GetComponentInChildren<CutsceneDriver>();
		_game = MetaManager.Instance.GetManager<GameManager>();
	}

	private void OnDestroy()
	{
		_game = null;
		_animator = null;
		_driver = null;
	}

	private void Update()
	{
		if(_driver.MustFollow)
			_game.Player.GoTo(_driver.transform.position);
	
		_game.Player.CurrentEmotion = _driver.MustShowEmotion ? 
				_emotion : BoomonController.Emotion.None;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != _game.Player.gameObject)
			return;

		_game.Player.IsControllable = false;
		_animator.SetTrigger(_playTriggerName);
	}



	#endregion

	public void OnEmotionClick(int index)
	{
		
	}


	public void OnResolutionClick(int index)
	{
		
	}

	//==================================================

	#region Private Fields

	[SerializeField] private BoomonController.Emotion _emotion;
	[SerializeField] private string _playTriggerName = "Play";
	[SerializeField] private string _playerTag = "Player";

	private GameManager _game;
	private Animator _animator;
	private CutsceneDriver _driver;

	#endregion

}
