﻿using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class DashSMB : SceneLinkedSMB<PlayerBehaviour>
    {
        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameManager.Instance.StatsManager.AddDodgeCountFor(animator.name);
            m_MonoBehaviour.Damageable.EnableInvulnerability(true);
            m_MonoBehaviour.GetInput();
            m_MonoBehaviour.RotateAim(m_MonoBehaviour.CInput);
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
