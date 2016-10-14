using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FacialAnimator : MonoBehaviour
{

	private void Awake()
	{
		var renderer = GetComponent<Renderer>();

		Debug.Assert(_facialMaterialIndex < renderer.sharedMaterials.Length,
			"FacialAnimator::Awake>> Index out of range!!", this);

		_facialMaterial = renderer.materials[_facialMaterialIndex];

		if (_facialBone == null)
			_facialBone = transform.parent.FindChild(_facialBoneName);

		enabled = _facialBone != null;
	}

	private void OnDestroy()
	{
		_facialMaterial = null;
		_facialBone = null;
	}

	private void Update()
	{
		//_facialMaterial.mainTextureOffset = _facialBone.position;

		_facialMaterial.mainTextureOffset = _facialBone.localPosition;
	}

	//===================================================================

	#region Private Fields

	private Material _facialMaterial;
	private Transform _facialBone;
	[SerializeField] private string _facialBoneName = "DummyFacial";
	[SerializeField] private int _facialMaterialIndex = 1;
	

	#endregion

}
