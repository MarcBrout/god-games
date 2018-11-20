﻿using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class HitSMB : SceneLinkedSMB<PlayerBehaviour>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Damageable.EnableInvulnerability(true);
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.Damageable.DisableInvulnerability();
        }

    }
}
