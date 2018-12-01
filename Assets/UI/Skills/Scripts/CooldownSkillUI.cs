using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SceneLinkedSMB;

namespace GodsGame
{
    public class CooldownSkillUI : MonoBehaviour
    {
        #region Public Var
        public Image cooldownOverlay;
        public TextMeshProUGUI chargeNumberText;
        [Tooltip("Look for the skillName in the TargetName script of type PlayerBehaviour")]
        public bool autoLookForSKillOnTarget = true;
        public string targetName;
        public string skillName;
        #endregion

        #region Private Var
        private CooldownSystem m_CooldownSystem;
        private Animator _Animator;
        private readonly int _HashStartCooldownPara = Animator.StringToHash("StartCooldown");
        private readonly int _HashEndCooldownPara = Animator.StringToHash("EndCooldown");
        #endregion

        #region Properties 
        public CooldownSystem CooldownSystem
        {
            get
            {
                return m_CooldownSystem;
            }
            set
            {
                if (m_CooldownSystem != null)
                    m_CooldownSystem.OnRemoveCharge -= TransitionToCooldownStart;
                m_CooldownSystem = value;
                m_CooldownSystem.OnRemoveCharge += TransitionToCooldownStart;
            }
        }
        #endregion

        public void Start()
        {
            if (autoLookForSKillOnTarget)
            {
                GameObject go = GameObject.Find(targetName);
                if (!go)
                    throw new System.Exception(this.name + ": can't find target named: " + targetName);
                PlayerBehaviour player = go.GetComponent<PlayerBehaviour>();
                if (!player)
                    throw new System.Exception(this.name + ": the target " + targetName + " has no component PlayerBehaviour");
                CooldownSkill<PlayerBehaviour> skill = player.GetPropValue<CooldownSkill<PlayerBehaviour>>(skillName);
                if (skill == null)
                    throw new System.Exception(this.name + ": the target " + targetName + " has no component Skill named: " + skillName);
                CooldownSystem = skill.CooldownSystem;
            }
            _Animator = GetComponent<Animator>();
            SceneLinkedSMB<CooldownSkillUI>.Initialise(_Animator, this);
            UpdateUI();
        }

        public void TransitionToCooldownEnd()
        {
            _Animator.SetTrigger(_HashEndCooldownPara);
        }

        public void TransitionToCooldownStart()
        {
            _Animator.SetTrigger(_HashStartCooldownPara);
        }

        public void UpdateUI()
        {
            if (CooldownSystem != null)
            {
                chargeNumberText.text = CooldownSystem.CurrentChargeNumber.ToString();
                cooldownOverlay.fillAmount = CooldownSystem.CurrentCooldownValue;
            }
        }
    }
}
