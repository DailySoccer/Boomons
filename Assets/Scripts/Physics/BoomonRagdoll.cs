

using UnityEngine;

public class BoomonRagdoll : Ragdoll
{

	#region Public Methods

	public override void Setup(Transform setupRef, ReferenceSystem refSystem)
	{
		base.Setup(setupRef, refSystem);
		_boomon = setupRef.GetComponent<BoomonController>();
	}

	public override void TeleportTo(Teleport target)
	{
		base.TeleportTo(target);
		if(_boomon != null)
			_boomon.TeleportTo(target);
	}
	
	#endregion

	//=============================================================

	#region Private Fields
	private BoomonController _boomon;
	#endregion

	
}

