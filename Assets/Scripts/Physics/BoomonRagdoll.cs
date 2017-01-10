public class BoomonRagdoll : Ragdoll
{

	#region Public Methods

	public void Setup(BoomonController boomon)
	{
		base.Init(boomon.transform, boomon.Setup.RagdollSetup, boomon.ReferenceSystem);
		_boomon = boomon;
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

