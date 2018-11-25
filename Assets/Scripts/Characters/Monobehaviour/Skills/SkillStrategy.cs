using UnityEngine;
using System;

namespace GodsGame
{
    public interface ISkillStrategy
    {
        void StartExecute(bool startCooldown);
        void UpdateExecute();
        void EndExecute();
    }

    public abstract class CooldownSkill<TMonoBehaviour> : MonoBehaviour, ISkillStrategy
        where TMonoBehaviour : MonoBehaviour
    {
        #region Protected Variables
        protected TMonoBehaviour m_MonoBehaviour;
        #endregion

        #region Public Variables
        [SerializeField]
        [Tooltip("Cooldown of the abillity in seconds")]
        private float m_Cooldown;
        [SerializeField]
        [Tooltip("Number of charged of the abillity")]
        private int m_MaxChargeNumber;
        public event Action OnExecute;
        #endregion

        #region Properties
        public CooldownSystem CooldownSystem { get; protected set; }
        public float Cooldown { get { return m_Cooldown; } protected set { m_Cooldown = value; } }
        public int MaxChargeNumber { get { return m_MaxChargeNumber; } protected set { m_MaxChargeNumber = value; } }
        #endregion

        private void Start()
        {
            m_MonoBehaviour = GetComponent<TMonoBehaviour>();
            CooldownSystem = new CooldownSystem(m_Cooldown, m_MaxChargeNumber);
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
        public virtual void StartExecute(bool startCooldown = true)
        {
            if (OnExecute != null)
                OnExecute();
            CooldownSystem.RemoveCharge();
            if (startCooldown && !CooldownSystem.IsOnCooldown() && m_MonoBehaviour != null)
                m_MonoBehaviour.StartCoroutine(CooldownSystem.StartCooldown());
        }

        public virtual void UpdateExecute() { }
        public virtual void EndExecute() { }

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