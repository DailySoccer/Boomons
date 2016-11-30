using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class FacialAnimator : MonoBehaviour
{

	public Vector2 FacialCoordinates
	{
		get {
			return  new Vector2(
				 _facialBone.localPosition.x, 
				-_facialBone.localPosition.y);
		}
		private set {
			_facialBone.localPosition = value;
		}
	}


	public void Reset()
	{
		FacialCoordinates = Vector3.zero;
	}

	//==============================================================


	private void Awake()
	{
		_animator = GetComponent<Animator>();

		if (_renderer == null)
			_renderer = GetComponentInChildren<SkinnedMeshRenderer>();

		Debug.Assert(_facialMaterialIndex < _renderer.sharedMaterials.Length,
			"FacialAnimator::Awake>> Index out of range!!", this);

		_facialMaterial = _renderer.materials[_facialMaterialIndex];

		if (_facialBone == null)
			_facialBone = transform.FindChild(_facialBoneName);

		enabled = _facialBone != null;
	}

	private void OnDestroy()
	{
		_renderer = null;
		_animator = null;
		_facialMaterial = null;
		_facialBone = null;
	}

	private void Update()
	{
		//_facialMaterial.mainTextureOffset = _facialBone.position;

		if(!_animator.IsInTransition(0))
			_facialMaterial.mainTextureOffset = FacialCoordinates;
	}


	//===================================================================

	#region Private Fields

	private Animator _animator;
	private Material _facialMaterial;
	private Transform _facialBone;
	[SerializeField] private SkinnedMeshRenderer _renderer;
	[SerializeField] private string _facialBoneName = "DummyFacial";
	[SerializeField] private int _facialMaterialIndex = 1;
	

	#endregion

}
