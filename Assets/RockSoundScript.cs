using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    [RequireComponent(typeof(AudioSource))]
    public class RockSoundScript : MonoBehaviour
    {
        public LayerMask hittableLayers;
        private AudioSource _audioSource;
        private Vector3 _lastPosition;

        private Damager _damager;
        // Use this for initialization
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _damager = GetComponent<Damager>();
        }

        private void Update()
        {
            float _speed = (transform.position - _lastPosition).magnitude / Time.deltaTime;
            _lastPosition = transform.position;
            if (_speed <= 0.1)
            {
                AudioManager.Instance.PlayRandomSfx3D("minotaur_step", ref _audioSource);
                if (_damager)
                {
                    _damager.DisableDamage();
                    _damager.enabled = false;
                }
                enabled = false;
            }
        }
    }
}