using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class FenceEvents : MonoBehaviour
    {
        public Animator _animator;
        private AudioSource _audioSource;

        private string _upParam = "GoUp";
        private string _downParam = "GoDown";

        // Use this for initialization
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void OnActivate(Activable activable, Trigger trigger)
        {
            _animator.SetTrigger(_downParam);
        }

        public void OnDeactivate(Activable activable, Trigger trigger)
        {
            _animator.SetTrigger(_upParam);
            if (_audioSource)
                AudioManager.Instance.PlaySfx3D("PRESSURE_PLATE_RESET", "items_pressure_plates", ref _audioSource);
        }
    }
}