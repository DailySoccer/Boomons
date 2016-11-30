public class BoomonProximityDetector : ProximityDetector
{

	protected virtual void OnEnable()
	{
		if(Game.Boomon != null)
			ProximityTarget = Game.Boomon.transform;
	}

	protected virtual void OnDisable()
	{
		ProximityTarget = null;
	}


	protected virtual void Start()
	{
		if(Game.Boomon != null)
			ProximityTarget = Game.Boomon.transform;
	}
	
}
