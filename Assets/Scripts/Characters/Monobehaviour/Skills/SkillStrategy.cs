using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GodsGame
{
    public interface ISkillStrategy
    {
        void Execute();
    }

    public abstract class CooldownSkill : ISkillStrategy
    {
        protected float cooldown = 3f;
        protected int maxChargeNumber = 1;
        protected int currentChargeNumber = 1;

        public float Cooldown { get { return cooldown; } }
        public int CurrentChargeNumber { get { return currentChargeNumber; } }
        public float RechargeValue { get; protected set; }
        public float RechargeEnd { get; protected set; }

        public CooldownSkill(float cooldown, int maxChargeNumber, int currentChargeNumber)
        {
            this.cooldown = cooldown;
            this.maxChargeNumber = maxChargeNumber;
            this.currentChargeNumber = currentChargeNumber;
        }

        public virtual void Execute()
        {
            currentChargeNumber -= 1;
        }

        public virtual bool CanUse()
        {
            return currentChargeNumber > 0;
        }

        public bool IsFullyCharged()
        {
            return currentChargeNumber >= maxChargeNumber;
        }

        public void StartCooldown()
        {
            RechargeEnd = Time.time + cooldown;
            RechargeValue = 0;
        }

        public void AddCharge()
        {
            currentChargeNumber += 1;
            if (currentChargeNumber > maxChargeNumber)
                currentChargeNumber = maxChargeNumber;
        }

        public void SetRechargeValue()
        {
            RechargeValue = 1 - ((RechargeEnd - Time.time) / cooldown);
        }

        public void EndCooldown()
        {
            RechargeValue = 0;
        }
    }
}