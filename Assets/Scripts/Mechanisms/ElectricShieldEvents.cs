using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class ElectricShieldEvents : MonoBehaviour
    {
        ParticleSystem _particleSystem;
        Collider _collider;
        private AudioSource _audioSource;

        // Use this for initialization
        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _collider = GetComponent<Collider>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void Disable(Activable activable, Trigger trigger)
        {
            _particleSystem.Stop();
            _collider.enabled = false;
            AudioManager.Instance.PlaySfx3D("shield_down", "items_common", ref _audioSource);
        }

        public void Enable(Activable activable, Trigger trigger)
        {
            AudioManager.Instance.PlaySfx3D("shield_up", "items_common", ref _audioSource);

            _particleSystem.Play();
            StartCoroutine(enableAfter());
        }

        public IEnumerator enableAfter()
        {
            yield return new WaitForSeconds(1000);
            _collider.enabled = true;
        }
    }
}