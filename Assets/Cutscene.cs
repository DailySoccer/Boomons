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
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject != _game.Player)
			return;

		_animator.SetTrigger(_playTriggerName);
		
	}

	#endregion

	//==================================================

	#region Private Fields

	
	[SerializeField] private string _playTriggerName = "Play";
	[SerializeField] private string _playerTag = "Player";

	private GameManager _game;
	private Animator _animator;
	#endregion

}
