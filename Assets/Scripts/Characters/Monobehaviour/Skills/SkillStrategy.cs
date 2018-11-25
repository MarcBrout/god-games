using UnityEngine;
using System;

namespace GodsGame
{
    public interface ISkillStrategy
    {
        void Execute(bool startCooldown);
    }

    public abstract class CooldownSkill<TMonoBehaviour> : ISkillStrategy 
        where TMonoBehaviour : MonoBehaviour
    {
        #region Protected Variables
        protected TMonoBehaviour m_MonoBehaviour;
        #endregion

        #region Public Variables
        public event Action OnExecute;
        #endregion

        #region Properties
        public CooldownSystem CooldownSystem { get; protected set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cooldown"></param>
        /// <param name="maxChargeNumber"></param>
        public CooldownSkill(TMonoBehaviour monoBehaviour, float cooldown = 3, int maxChargeNumber = 1)
        {
            CooldownSystem = new CooldownSystem(cooldown, maxChargeNumber);
            m_MonoBehaviour = monoBehaviour;
        }

        public void AssignUser(TMonoBehaviour monoBehaviour)
        {
            m_MonoBehaviour = monoBehaviour;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cooldown"></param>
        /// <param name="maxChargeNumber"></param>
        public CooldownSkill(float cooldown = 3, int maxChargeNumber = 1)
        {
            CooldownSystem = new CooldownSystem(cooldown, maxChargeNumber);
        }

        /// <summary>
        /// Implement the skill in this function
        /// </summary>
        public virtual void Execute(bool startCooldown = true)
        {
            if (OnExecute != null)
                OnExecute();
            CooldownSystem.RemoveCharge();
            if (startCooldown && !CooldownSystem.IsOnCooldown() && m_MonoBehaviour != null)
               m_MonoBehaviour.StartCoroutine(CooldownSystem.StartCooldown());
        }

        /// <summary>
        /// The skill need to have at least one charge to be able to use it
        /// </summary>
        /// <returns></returns>
        public virtual bool CanUse()
        {
            return CooldownSystem.HasCharge();
        }
    }
}