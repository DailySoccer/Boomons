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
		if (value)
		{
			base.OnInteractableChange(true);
			ProximityTarget = null;
		}
	}

	private void Shoot(Toucher toucher, Vector2 pos)
	{
		Animator.SetTrigger(_shootTriggerName);
		Audio.PlayOneShot(_shootClip);
		enabled = false;
	}

	#endregion


	//===========================================================

	#region Private Fields

	[SerializeField] private string _shootTriggerName = "Shoot";
	[SerializeField] private AudioClip _shootClip;

	#endregion
}
