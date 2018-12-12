using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class RopeEvents : MonoBehaviour {

        public ParticleSystem HitEffect;
        AudioSource _audioSource;

        // Use this for initialization
        void Start() {
            _audioSource = GetComponent<AudioSource>();
            if (HitEffect == null)
            {
                Debug.LogWarning("HitEffect not set on " + gameObject.name);
            }
        }

        public void OnDamage(Damager damager, Damageable damageable)
        {
            AudioManager.Instance.PlaySfx3D("hit_on_rope", "items_common", ref _audioSource);
            if (HitEffect)
            {
                HitEffect.Play();
            }
        }

        public void OnDie(Damager damager, Damageable damageable)
        {
            AudioManager.Instance.PlaySfx3D("rope_break", "items_common", ref _audioSource);
            if (HitEffect)
            {
                HitEffect.Play();
            }
        }
    }
}