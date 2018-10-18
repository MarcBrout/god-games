using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    [RequireComponent(typeof(AudioSource))]
    public class PressurePlateEvents : MonoBehaviour
    {
        public Animator _animator;

        private AudioSource _audioSource;

        private string PLATE_ARRAY = "items_pressure_plates";
        private string PLATE_ACTIVATED = "PRESSURE_PLATE_ACTIVATED";
        private string PLATE_RESET = "PRESSURE_PLATE_RESET";
        private string PLATE_INACTIVE = "PRESSURE_PLATE_INACTIVE";

        // Use this for initialization
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator.SetBool("isEnable", true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnEnter(Activable activable, Trigger trigger)
        {
            AudioManager.Instance.PlaySfx3D(PLATE_ACTIVATED, PLATE_ARRAY, ref _audioSource);
            _animator.SetBool("playerEntered", true);
        }

        public void OnEnterDisabled(Activable activable, Trigger trigger)
        {
            AudioManager.Instance.PlaySfx3D(PLATE_INACTIVE, PLATE_ARRAY, ref _audioSource);
        }

        public void OnExit(Activable activable, Trigger trigger)
        {
            _animator.SetBool("playerEntered", false);
          
        }
    }
}