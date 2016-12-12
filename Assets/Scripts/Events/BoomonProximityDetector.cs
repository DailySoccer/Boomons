public class BoomonProximityDetector : ProximityDetector
{
	protected virtual void OnEnable()
	{
		if(Game.Boomon != null)
			ProximityTarget = Game.Boomon.transform;
	}

	protected override void Start()
	{
		if(Game.Boomon != null)
			ProximityTarget = Game.Boomon.transform;
	}

}
