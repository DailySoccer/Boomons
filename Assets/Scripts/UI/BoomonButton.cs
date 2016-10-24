using UnityEngine;

[ExecuteInEditMode]
public class BoomonButton : MonoBehaviour
{

	#region Public Fields

	public BoomonRole? BoomonRole
	{
		get { return _boomonRole; }
		private set
		{
			if (!value.HasValue || value == _boomonRole)
				return;

			string prefabPath = PathSolver.Instance.GetBoomonPath(value.Value,
				PathSolver.InstanceType.NotControllable);

			if (_boomon != null)
				Destroy(_boomon.gameObject);

			_boomon = ((GameObject)Instantiate(Resources.Load(prefabPath), _anchor))
				.GetComponent<BoomonController>();

			_boomonRole = value;
		}
	}

	#endregion

	//==========================================

	#region Public Methods

	public void Setup(BoomonRole role)
	{
		BoomonRole = role;
	}


	#endregion

	//===================================================================

	#region Mono

	private void Awake()
	{
		if (_anchor == null) {
			_anchor = new GameObject("Anchor").transform;
			_anchor.parent = transform;
		}
	}

	private void OnDestroy()
	{
		_anchor = null;
		_boomon = null;
	}

#if UNITY_EDITOR
	private void Update()
	{
		BoomonRole = _boomonRoleEditor;
	}
#endif

	#endregion

	//===================================================================

	#region Private Fields

	[SerializeField] private Transform _anchor;
	private BoomonController _boomon;
	private BoomonRole? _boomonRole;

#if UNITY_EDITOR
	[SerializeField] private BoomonRole _boomonRoleEditor;
#endif
	#endregion
}
