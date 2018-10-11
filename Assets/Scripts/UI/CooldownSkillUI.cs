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
        public Image cooldownOverlay;
        public TextMeshProUGUI chargeNumberText;
        public string targetName;
        public string skillName;

        public CooldownSkill Skill { get; set; }

        private Animator _Animator;
        private readonly int _HashStartCooldownPara = Animator.StringToHash("StartCooldown");
        private readonly int _HashEndCooldownPara = Animator.StringToHash("EndCooldown");
   
        public void Start()
        {
            PlayerBehaviour player = GameObject.Find(targetName).GetComponent<PlayerBehaviour>();
            Skill = player.GetPropValue<CooldownSkill>(skillName);
            Skill.OnExecute += TransitionToCooldownStart;
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
            if (Skill != null)
            {
                chargeNumberText.text = Skill.CurrentChargeNumber.ToString();
                cooldownOverlay.fillAmount = Skill.CurrentCooldownValue;
            }
        }
    }
}
