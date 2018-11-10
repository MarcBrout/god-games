using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace GodsGame
{
    [RequireComponent(typeof(Collider))]
    public class Damager : MonoBehaviour
    {
        [Serializable]
        public class DamagableEvent : UnityEvent<Damager, Damageable>
        { }

        [Serializable]
        public class NonDamagableEvent : UnityEvent<Damager>
        { }

        //call that from inside the onDamageableHIt or OnNonDamageableHit to get what was hit.
        public Collider LastHit { get { return m_LastHit; } }
        public bool trigger = true;
        public bool m_CanDamage = true;
        public int damage = 1;
        public bool enableDelayActivationOnStart = false;
        public float activationDelay = 0.1f;
        public bool enableDelayDeactivationOnStart = false;
        public float deactivationDelay = 3f;
        public bool damageOverTime = false;
        public float damageOverTimeTick = 1f;
        public bool disableDamageAfterHit = false;
        [Tooltip("If set, an invincible damageable hit will still get the onHit message (but won't loose any life)")]
        public bool ignoreInvincibility = false;
        public LayerMask hittableLayers;
        public DamagableEvent OnDamageableHit;
        public NonDamagableEvent OnNonDamageableHit;
        public Collider Collider {
            get
            {
                return m_Collider;
            }
        }

        protected Collider m_Collider;
        protected bool m_SpriteOriginallyFlipped;
        protected Transform m_DamagerTransform;
        protected Collider m_LastHit;
        protected float m_TriggerEnterAt;
        protected float m_TriggerStayElapseTime;

        void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            m_DamagerTransform = transform;
            if (!m_Collider)
                m_Collider = GetComponent<Collider>();
            if (enableDelayActivationOnStart)
            {
                m_Collider.enabled = false;
                StartCoroutine(ActivateAfter(activationDelay));
            }
            if (enableDelayDeactivationOnStart)
                StartCoroutine(DisableAfter(deactivationDelay));
        }

        public void EnableDamage()
        {
            m_CanDamage = true;
        }

        public void DisableDamage()
        {
            m_CanDamage = false;
        }

        public void DisableObject()
        {
            gameObject.SetActive(false);
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (!m_CanDamage)
                return;
            if (!trigger && hittableLayers.Contain(collision.gameObject.layer))
            {
                m_LastHit = collision.collider;
                ApplyDamage();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_CanDamage)
                return;
            if (trigger && hittableLayers.Contain(other.gameObject.layer))
            {
                m_LastHit = other;
                ApplyDamage();
            }

        }

        private void OnTriggerStay(Collider other)
        {
            if (trigger && m_CanDamage && damageOverTime && m_TriggerEnterAt + damageOverTimeTick <= Time.time)
                ApplyDamage();
        }

        private void ApplyDamage()
        {
            m_TriggerEnterAt = Time.time;
            Damageable damageable = m_LastHit.GetComponent<Damageable>();
            if (damageable)
            {
                OnDamageableHit.Invoke(this, damageable);
                damageable.TakeDamage(this, ignoreInvincibility);
                if (disableDamageAfterHit)
                    DisableDamage();
            }
            else
            {
                OnNonDamageableHit.Invoke(this);
            }
        }

        public IEnumerator ActivateAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            m_Collider.enabled = true;
        }

        public IEnumerator DisableAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            DisableObject();
        }
    }
}
