using UnityEngine;
	
[RequireComponent(typeof(Toucher))]
public class ShootTarget : Item
{
	#region Mono

	protected override void OnEnable()
	{
		Toucher.enabled = true;
		base.OnEnable();
		Toucher.TapStart.AddListener(Shoot);
	}

	protected override void OnDisable()
	{
		Toucher.TapStart.RemoveListener(Shoot);
		base.OnDisable();
		Toucher.enabled = false;
	}

	#endregion

	//======================================================

	#region Events

	protected override void OnInteractableChange(bool value)
	{
		if (!value || _canBeShot)
			return;

		base.OnInteractableChange(true);
		_canBeShot = true;
	}

	private void Shoot(Toucher toucher, Vector2 pos)
	{
		if (!_canBeShot)
			return;

		Animator.SetTrigger(_shootTriggerName);
		AudioSource.PlayOneShot(_shootClip);
		_canBeShot = false;
	}

	#endregion


	//===========================================================

	#region Private Fields

	[SerializeField] private string _shootTriggerName = "Shoot";
	[SerializeField] private AudioClip _shootClip;
	private bool _canBeShot;

	#endregion
}
