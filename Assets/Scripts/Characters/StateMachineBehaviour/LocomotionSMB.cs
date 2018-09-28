using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame {

    public class LocomotionSMB : SceneLinkedSMB<PlayerBehaviour> {

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.GetInput();
            m_MonoBehaviour.CheckForGrounded();
            m_MonoBehaviour.Move();
            m_MonoBehaviour.RotateAim();
            if (m_MonoBehaviour.CheckForDashInput())
            {
                Debug.Log("Dash input true");
                m_MonoBehaviour.TransitionToDash();
            }
            if (m_MonoBehaviour.CheckForJumpInput())
                m_MonoBehaviour.Jump();
        }
    }
}
