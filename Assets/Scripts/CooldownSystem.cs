using System;
using System.Collections;
using UnityEngine;

namespace GodsGame
{
    public class CooldownSystem
    {
        #region Public Variables
        public event Action OnRemoveCharge;
        #endregion

        #region Protected Variables
        protected int m_MaxChargeNumber = 1;
        #endregion

        #region Properties
        public float Cooldown { get; protected set; }
        public int CurrentChargeNumber { get; protected set; }
        public float CurrentCooldownValue { get; protected set; }
        public float CooldownEndTime { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cooldown"></param>
        /// <param name="maxChargeNumber"></param>
        public CooldownSystem( float cooldown = 3, int maxChargeNumber = 1)
        {
            Cooldown = cooldown;
            m_MaxChargeNumber = maxChargeNumber;
            CurrentChargeNumber = maxChargeNumber;
        }

        /// <summary>
        /// The skill need to have at least one charge to be able to use it
        /// </summary>
        /// <returns></returns>
        public virtual bool HasCharge()
        {
            return CurrentChargeNumber > 0;
        }

        /// <summary>
        /// Check if the skill charge are full
        /// </summary>
        /// <returns></returns>
        public bool IsFullyCharged()
        {
            return CurrentChargeNumber >= m_MaxChargeNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void RemoveCharge()
        {
            if (OnRemoveCharge != null)
                OnRemoveCharge();
            CurrentChargeNumber -= 1;
            if (CurrentChargeNumber < 0)
                CurrentChargeNumber = 0;
        }

        /// <summary>
        /// Increment the skill charge by one
        /// </summary>
        public void AddCharge()
        {
            CurrentChargeNumber += 1;
            if (CurrentChargeNumber > m_MaxChargeNumber)
                CurrentChargeNumber = m_MaxChargeNumber;
        }

        /// <summary>
        /// Call this function when you use the skill to set the cooldown
        /// </summary>
        public void InitCooldown()
        {
            CooldownEndTime = Time.time + Cooldown;
            CurrentCooldownValue = 0;
        }

        /// <summary>
        /// Call this function to set the currentCooldownValue compared to the global cooldown the value is between 0 and 1
        /// </summary>
        public void SetCurrentCooldownValue()
        {
            CurrentCooldownValue = 1 - ((CooldownEndTime - Time.time) / Cooldown);
        }

        /// <summary>
        /// Reset the cooldownValue
        /// </summary>
        public void EndCooldown()
        {
            CurrentCooldownValue = 0;
        }

        public bool IsOnCooldown()
        {
            return Time.time < CooldownEndTime;
        }

        /// <summary>
        /// Basic implementation of the cooldown mechanics
        /// You can use the function of the class to implement your own
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartCooldown()
        {
            while (!IsFullyCharged())
            {
                InitCooldown();
                while (IsOnCooldown())
                {
                    SetCurrentCooldownValue();
                    yield return new WaitForEndOfFrame();
                }
                AddCharge();
                yield return new WaitForEndOfFrame();
            }
            EndCooldown();
        }
    }
}
