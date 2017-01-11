public class BoomonProximityDetector : ProximityDetector
{

	#region Mono

	protected override void OnDestroy()
	{
		if (_boomon != null) {
			_boomon.StateChange -= OnBoomonStateChange;
			_boomon = null;
		}
		base.OnDestroy();
	}
	
	protected override void Start()
	{
		_boomon = Game.Boomon;
		_boomon.StateChange += OnBoomonStateChange;
		ProximityTarget = _boomon.Transform;
	}


	#endregion

	//======================================================================

	#region Events

	private void OnBoomonStateChange(BoomonController.State last, BoomonController.State next)
	{
		if(last == BoomonController.State.Throw || next == BoomonController.State.Throw)
			ProximityTarget = _boomon.Transform;
	}

	#endregion


	//===========================================================

	#region Private Fields
	private BoomonController _boomon;
	#endregion

}
