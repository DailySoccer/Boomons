using UnityEngine;

public class BoomonFollower : ScrollFollower
{

	#region Public Fields

	public override Transform Target  {
		get { return _game.Boomon.Transform; }
	}

	#endregion

	//==============================================

	#region Mono

	protected override void Awake()
	{
		base.Awake();
		_game = MetaManager.Instance.Get<GameManager>();
	}

	protected override void OnDestroy()
	{
		_game = null;
		base.OnDestroy();
	}

	#endregion

	//===========================================================

	#region Private Fields

	protected override ReferenceSystem RefSystem {
		get { return _game.Boomon.ReferenceSystem; }
	}

	private GameManager _game;

	#endregion
}
