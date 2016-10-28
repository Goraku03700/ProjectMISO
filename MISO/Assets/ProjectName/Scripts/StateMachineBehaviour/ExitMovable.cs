using UnityEngine;
using System.Collections;

public class ExitMovable : StateMachineBehaviour {

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Movable movable = animator.GetComponent<Movable>();

        movable.enabled = false;
	}
}
