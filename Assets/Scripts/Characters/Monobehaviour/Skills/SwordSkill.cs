﻿using UnityEngine;

namespace GodsGame
{
    public class SwordSkill : CooldownSkill<PlayerBehaviour>
    {
        public SwordSkill(float cooldown = 3, int maxChargeNumber = 1) : base(cooldown, maxChargeNumber)
        {
        }

        public override void Execute(bool startCooldown)
        {
            base.Execute(startCooldown);
            Debug.Log("Sword Slash Effect : WOW");
        }
    }
}