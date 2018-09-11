using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLinkedSMB;

namespace GodsGame
{
    public class CooldownSkillSMB : SceneLinkedSMB<CooldownSkillUI>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateEnter(animator, stateInfo, layerIndex);
            if (!m_MonoBehaviour.Skill.IsFullyCharged())
                m_MonoBehaviour.Skill.StartCooldown();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
            if (Time.time >= m_MonoBehaviour.Skill.RechargeEnd)
            {
                m_MonoBehaviour.Skill.AddCharge();
                if (!m_MonoBehaviour.Skill.IsFullyCharged())
                    m_MonoBehaviour.Skill.StartCooldown();
                else
                    m_MonoBehaviour.TransitionToCooldownEnd();
            }
            else
            {
                m_MonoBehaviour.Skill.SetRechargeValue();
                m_MonoBehaviour.UpdateUI();
            }
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateExit(animator, stateInfo, layerIndex);
            m_MonoBehaviour.Skill.EndCooldown();
            m_MonoBehaviour.UpdateUI();
        }
    }
}
