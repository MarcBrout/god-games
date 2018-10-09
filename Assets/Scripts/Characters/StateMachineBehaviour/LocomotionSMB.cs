using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame {

    public class LocomotionSMB : SceneLinkedSMB<PlayerBehaviour> {

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.GetInput();
            m_MonoBehaviour.CheckForGrounded();
            m_MonoBehaviour.Move();
            m_MonoBehaviour.RotateAim();
            m_MonoBehaviour.DoStepDust();
            Debug.Log("checking for dash");
            if (m_MonoBehaviour.CheckForDashInput())
            {
                m_MonoBehaviour.TransitionToDash();
            }
            if (m_MonoBehaviour.CheckForJumpInput())
                m_MonoBehaviour.Jump();

            if (m_MonoBehaviour.CheckForThrowInput())
            {
                Debug.Log("Throw input true");

                m_MonoBehaviour.ThrowItem();

            }
        }
    }
}
