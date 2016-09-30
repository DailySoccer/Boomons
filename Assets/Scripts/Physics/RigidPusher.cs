using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class RigidPusher : MonoBehaviour
{

	
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;

		if (hit.moveDirection.y < -0.3F)
			return;

		body.AddForce(hit.moveDirection * _pushPower, ForceMode.Impulse);
	}


	[SerializeField, Range(0f, 100f)]
	private float _pushPower = 2.0F;
}
