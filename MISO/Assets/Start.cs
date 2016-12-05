using UnityEngine;
using System.Collections;

namespace Players
{
    public class Start : StateMachineBehaviour
    {

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_playerCharacter = animator.gameObject.GetComponent<PlayerCharacter>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_playerCharacter.StartExit();
        }

        private PlayerCharacter m_playerCharacter;
    }

}