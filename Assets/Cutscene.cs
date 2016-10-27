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

		_game.Player.SetIsControllable(false);
		_animator.SetTrigger(_playTriggerName);
	}



	#endregion


	//===================================================================

	#region Events

	public void OnEmotionClick(int index)
	{
		Debug.Log("Cutscene::OnEmotionClick>> " + index);
		_animator.SetInteger(_emotionIntName, index);
	}


	public void OnResolutionClick(int index)
	{
		Debug.Log("Cutscene::OnEmotionClick>> " + index);
		_animator.SetInteger(_resolutionIntName, index);
	}


	#endregion


	//==================================================

	#region Private Fields

	[SerializeField] private BoomonController.Emotion _emotion;
	[SerializeField] private string _playTriggerName = "Play";
	[SerializeField] private string _playerTag = "Player";
	[SerializeField] private string _emotionIntName = "Emotion";
	[SerializeField] private string _resolutionIntName = "Resolution";

	private GameManager _game;
	private Animator _animator;
	private CutsceneDriver _driver;
	

	#endregion

}
