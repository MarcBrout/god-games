using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class IdleSMB : SceneLinkedSMB<PlayerBehaviour>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CheckForGrounded();
            m_MonoBehaviour.GetInput();
            m_MonoBehaviour.GroundedHorizontalMovement(true);
            m_MonoBehaviour.RotateAim();
            if (m_MonoBehaviour.CheckForJumpInput())
                m_MonoBehaviour.Jump();
            if (m_MonoBehaviour.CheckForThrowInput())
                m_MonoBehaviour.ThrowItem();
            if (m_MonoBehaviour.CheckForUseItemInput())
                m_MonoBehaviour.UseItem();
        }
    }
}
