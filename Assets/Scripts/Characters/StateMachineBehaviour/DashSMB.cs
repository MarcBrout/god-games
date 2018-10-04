using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class DashSMB : SceneLinkedSMB<PlayerBehaviour>
    {
        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Damageable.EnableInvulnerability();
            m_MonoBehaviour.GetInput();
            m_MonoBehaviour.Dash();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.CheckForGrounded();
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Damageable.DisableInvulnerability();
        }
    }
}
