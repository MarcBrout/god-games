using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class ElectricShieldEvents : MonoBehaviour
    {
        ParticleSystem _particleSystem;
        Collider _collider;

        // Use this for initialization
        void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _collider = GetComponent<Collider>();
        }

        public void Disable(Activable activable, Trigger trigger)
        {
            _particleSystem.Stop();
            _collider.enabled = false;
        }

        public void Enable(Activable activable, Trigger trigger)
        {
            _particleSystem.Play();
            _collider.enabled = true;
        }
    }
}