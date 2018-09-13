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
        public float CurrentCooldownValue { get; protected set; }
        public float CooldownEndTime { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cooldown"></param>
        /// <param name="maxChargeNumber"></param>
        /// <param name="currentChargeNumber"></param>
        public CooldownSkill(float cooldown, int maxChargeNumber, int currentChargeNumber)
        {
            this.cooldown = cooldown;
            this.maxChargeNumber = maxChargeNumber;
            this.currentChargeNumber = currentChargeNumber;
        }

        /// <summary>
        /// Implement the skill in this function
        /// </summary>
        public virtual void Execute()
        {
            currentChargeNumber -= 1;
        }

        /// <summary>
        /// The skill need to have at least one charge to be able to use it
        /// </summary>
        /// <returns></returns>
        public virtual bool CanUse()
        {
            return currentChargeNumber > 0;
        }

        /// <summary>
        /// Check if the skill charge are full
        /// </summary>
        /// <returns></returns>
        public bool IsFullyCharged()
        {
            return currentChargeNumber >= maxChargeNumber;
        }

        /// <summary>
        /// Increment the skill charge by one
        /// </summary>
        public void AddCharge()
        {
            currentChargeNumber += 1;
            if (currentChargeNumber > maxChargeNumber)
                currentChargeNumber = maxChargeNumber;
        }

        /// <summary>
        /// Call this function when you use the skill to set the cooldown
        /// </summary>
        public void StartCooldown()
        {
            CooldownEndTime = Time.time + cooldown;
            CurrentCooldownValue = 0;
        }

        /// <summary>
        /// Call this function to set the currentCooldownValue compared to the global cooldown
        /// </summary>
        public void SetCurrentCooldownValue()
        {
            CurrentCooldownValue = 1 - ((CooldownEndTime - Time.time) / cooldown);
        }

        /// <summary>
        /// Reset the cooldownValue
        /// </summary>
        public void EndCooldown()
        {
            CurrentCooldownValue = 0;
        }

        /// <summary>
        /// Basic implementation of the reload
        /// You can use the function of the class to implement your own
        /// </summary>
        /// <returns></returns>
        public IEnumerator Reload()
        {
            while (!IsFullyCharged())
            {
                StartCooldown();
                while (Time.time < CooldownEndTime)
                {
                    SetCurrentCooldownValue();
                    yield return new WaitForEndOfFrame();
                }
                AddCharge();
                yield return null;
            }
            EndCooldown();
        }
    }
}