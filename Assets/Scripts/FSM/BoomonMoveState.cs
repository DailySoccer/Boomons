using UnityEngine;

public class BoomonMoveState : StateMachineBehaviour
{
	[SerializeField] private AudioClip _leftStep;
	[SerializeField, Range(0f, 1f)] private float _leftStepTime;
	[SerializeField] private AudioClip _rightStep;
	[SerializeField, Range(0f, 1f)] private float _rightStepTime;
	private int _playedClipCount = 0;


	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_playedClipCount == 1 && stateInfo.normalizedTime > _rightStepTime)
			Play(_rightStep, animator.transform.position);
		else if (_playedClipCount == 0 && stateInfo.normalizedTime > _leftStepTime)
			Play(_leftStep, animator.transform.position);
		else if (_playedClipCount == 2 && stateInfo.normalizedTime < _leftStepTime)
			_playedClipCount = 0;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}


	private void Play(AudioClip leftStep, Vector3 position)
	{
		++_playedClipCount;
		AudioSource.PlayClipAtPoint(_leftStep, position);
	}
}
