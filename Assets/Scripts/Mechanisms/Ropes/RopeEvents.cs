using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class RopeEvents : MonoBehaviour {

        AudioSource _audioSource;

        // Use this for initialization
        void Start() {
            _audioSource = GetComponent<AudioSource>();
        }

        public void onDamage(Damager damager, Damageable damageable)
        {
            AudioManager.Instance.PlaySfx3D("hit_on_rope", "items_common", ref _audioSource);
        }

        public void onDie(Damager damager, Damageable damageable)
        {
            AudioManager.Instance.PlaySfx3D("rope_break", "items_common", ref _audioSource);
        }
    }
}