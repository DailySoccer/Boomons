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

			if (_boomonAnim != null)
				DestroyImmediate(_boomonAnim.gameObject);

			_boomonAnim = ((GameObject)Instantiate(Resources.Load(prefabPath), Anchor))
				.GetComponent<Animator>();

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
	}

	private void OnDestroy()
	{
		Anchor = null;
		_boomonAnim = null;
	}

#if UNITY_EDITOR
	private void Update()
	{
		BoomonRole = _boomonRoleEditor;
	}
#endif

	#endregion

	//============================================================

	#region Callbacks

	public void OnClick()
	{
		
	}

	public void OnHover()
	{
		
	}

	#endregion

	//===================================================================

	#region Private Fields

	private const string AnchorName = "Anchor";

	private Transform Anchor
	{
		get
		{
			if (_anchor == null) {
				_anchor = transform.FindChild(AnchorName);
				if (_anchor == null) {
					_anchor = new GameObject(AnchorName).transform;
					_anchor.parent = transform;
				}
			}
			return _anchor;
		}
		set { _anchor = value; }
	}


	[SerializeField] private Transform _anchor;
	
	private Animator _boomonAnim;
	private BoomonRole? _boomonRole;

#if UNITY_EDITOR
	[SerializeField] private BoomonRole _boomonRoleEditor;
#endif

	#endregion
}
