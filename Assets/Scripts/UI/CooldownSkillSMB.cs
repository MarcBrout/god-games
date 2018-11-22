using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class CooldownSkillSMB : SceneLinkedSMB<CooldownSkillUI>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (m_MonoBehaviour.CooldownSystem.IsOnCooldown())
            {
                m_MonoBehaviour.UpdateUI();
            }
            else if (m_MonoBehaviour.CooldownSystem.IsFullyCharged())
            {
                m_MonoBehaviour.TransitionToCooldownEnd();
            }
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.UpdateUI();
        }
    }
}
