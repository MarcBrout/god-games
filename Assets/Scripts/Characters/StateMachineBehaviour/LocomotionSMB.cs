using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame {

    public class LocomotionSMB : SceneLinkedSMB<PlayerBehaviour> {

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CheckForGrounded();
            m_MonoBehaviour.GetInput();
            m_MonoBehaviour.RotateAim();
            m_MonoBehaviour.GroundedHorizontalMovement(true);
            m_MonoBehaviour.DoStepDust();
            if (m_MonoBehaviour.CheckForIdle())
                m_MonoBehaviour.GoToIdleState();
            if (m_MonoBehaviour.CheckForDashInput())
                m_MonoBehaviour.TransitionToDash();
            if (m_MonoBehaviour.CheckForJumpInput())
                m_MonoBehaviour.Jump();
            if (m_MonoBehaviour.CheckForThrowInput()) 
                m_MonoBehaviour.ThrowItem();
            if (m_MonoBehaviour.CheckForUseItemInput())
                m_MonoBehaviour.UseItem();
        }
    }
}
