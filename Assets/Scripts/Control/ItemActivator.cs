using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ItemActivator : Touchable
{

	#region Public Methods

	private void Activate()
	{
		Debug.Log("ItemActivator::Activate");
		
		_animator.SetTrigger(_playTriggerName);
	}

	#endregion

	//======================================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();
		_animator = GetComponent<Animator>();
	}

	protected override void OnDestroy()
	{
		_animator = null;
		base.OnDestroy();
	}

	#endregion

	//======================================================================

	#region Events

	public override void OnTapStart(GameObject go, Vector2 touchPos)
	{
		Activate();
	}

	
	#endregion

	//====================================================

	#region Private Fields

	[SerializeField] private Animator _animator;
	[SerializeField] private string _playTriggerName = "Play";

	#endregion
}
