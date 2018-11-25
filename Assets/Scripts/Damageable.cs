using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace GodsGame
{
    public class Damageable : MonoBehaviour
    {
        [Serializable]
        public class HealthEvent : UnityEvent<Damageable>
        { }

        [Serializable]
        public class DamageEvent : UnityEvent<Damager, Damageable>
        { }

        [Serializable]
        public class HealEvent : UnityEvent<int, Damageable>
        { }

        public int startingHealth = 5;
        public bool invulnerableAfterDamage = true;
        public float invulnerabilityDuration = 3f;
        public bool disableOnDeath = false;
        [Tooltip("Disable delay after death")]
        public float disableAfterMilliseconds = 1000f;
        [Tooltip("An offset from the object position used to set from where the distance to the damager is computed")]
        public Vector2 centerOffset = new Vector2(0f, 0f);
        public HealthEvent OnHealthSet;
        public DamageEvent OnTakeDamage;
        public DamageEvent OnTakeDamageBt;
        public DamageEvent OnDie;
        public HealEvent OnGainHealth;

        //public DataSettings dataSettings;
        protected int m_CurrentHealth;
        protected bool m_Invulnerable;
        protected float m_InulnerabilityTimer;
        protected Vector2 m_DamageDirection;
        protected bool m_ResetHealthOnSceneReload;

        public int CurrentHealth
        {
            get { return m_CurrentHealth; }
        }

        void OnEnable()
        {
            m_CurrentHealth = startingHealth;
            OnHealthSet.Invoke(this);
            DisableInvulnerability();
        }

        //void Update()
        //{
        //    if (m_Invulnerable)
        //    {
        //        m_InulnerabilityTimer -= Time.deltaTime;

        //        if (m_InulnerabilityTimer <= 0f)
        //        {
        //            m_Invulnerable = false;
        //        }
        //    }
        //}

        public void EnableInvulnerability(bool ignoreTimer = false)
        {
            m_Invulnerable = true;
            //technically don't ignore timer, just set it to an insanly big number. Allow to avoid to add more test & special case.
            m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
        }

        public void DisableInvulnerability()
        {
            m_Invulnerable = false;
        }

        public Vector2 GetDamageDirection()
        {
            return m_DamageDirection;
        }

        public void TakeDamage(Damager damager, bool ignoreInvincible = false)
        {
            Debug.Log("Take damage incicible " + m_Invulnerable + " ignore " + ignoreInvincible + " currentHeath " + m_CurrentHealth);
            if ((m_Invulnerable && !ignoreInvincible) || m_CurrentHealth <= 0)
                return;

            //we can reach that point if the damager was one that was ignoring invincible state.
            //We still want the callback that we were hit, but not the damage to be removed from health.
            if (!m_Invulnerable)
            {
                m_CurrentHealth -= damager.damage;
                OnHealthSet.Invoke(this);
            }

            //m_DamageDirection = transform.position + (Vector3)centerOffset - damager.transform.position;
            OnTakeDamage.Invoke(damager, this);
            OnTakeDamageBt.Invoke(damager, this);

            if (m_CurrentHealth <= 0)
            {
                OnDie.Invoke(damager, this);
                m_ResetHealthOnSceneReload = true;
                EnableInvulnerability();
                if (disableOnDeath) {
                    StartCoroutine(DisableAfterSeconds(disableAfterMilliseconds));
                }
            }
        }

        public void GainHealth(int amount)
        {
            m_CurrentHealth += amount;
            if (m_CurrentHealth > startingHealth)
                m_CurrentHealth = startingHealth;
            OnHealthSet.Invoke(this);
            OnGainHealth.Invoke(amount, this);
        }

        public void SetHealth(int amount)
        {
            m_CurrentHealth = amount;
            OnHealthSet.Invoke(this);
        }

        public IEnumerator DisableAfterSeconds(float milliseconds)
        {
            yield return new WaitForSeconds(milliseconds / 1000f);
            gameObject.SetActive(false);
        }
    }
}
