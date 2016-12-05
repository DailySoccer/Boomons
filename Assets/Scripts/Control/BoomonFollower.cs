using UnityEngine;

public class BoomonFollower : ScrollFollower
{

	#region Public Fields

	public override Transform Target   {
		get {
			return Boomon != null ? 
				Boomon.Transform :
				base.Target;
		}
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
		_boomon = null;
		base.OnDestroy();
	}

	#endregion

	//===========================================================

	#region Private Fields

	protected override ReferenceSystem RefSystem {
		get {
			return Boomon != null
				? Boomon.ReferenceSystem
				: base.RefSystem;
		}
	}

	protected BoomonController Boomon {
		get { return _boomon ?? (_boomon = _game.Boomon); }
	}

	private GameManager _game;
	private BoomonController _boomon;

	#endregion

	
}
