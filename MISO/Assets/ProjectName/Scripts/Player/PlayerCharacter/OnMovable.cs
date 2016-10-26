using UnityEngine;
using System.Collections;

public class OnMovable : StateMachineBehaviour {

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Movable movable = animator.gameObject.GetComponent<Movable>();

        movable.enabled = true;
    }
}
