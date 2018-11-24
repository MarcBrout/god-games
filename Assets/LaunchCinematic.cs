using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class LaunchCinematic : MonoBehaviour
    { 
        private Animator _pressurPlateAnimator;
        private AudioSource _audioSource;
        public LaunchAnimationOnTrigger Trigger;

        // Sounds Names
        private string PLATE_ARRAY = "items_pressure_plates";
        private string PLATE_ACTIVATED = "PRESSURE_PLATE_ACTIVATED";

        // Use this for initialization
        void Start()
        {
            _pressurPlateAnimator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }


        public void PlateTriggerEnter(Collider col)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                AudioManager.Instance.PlaySfx3D(PLATE_ACTIVATED, PLATE_ARRAY, ref _audioSource);
                Trigger.AddTriggerCount();
            }
        }

        public void PlateTriggerExit(Collider col)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                AudioManager.Instance.PlaySfx3D(PLATE_ACTIVATED, PLATE_ARRAY, ref _audioSource);
                Trigger.SubstractTriggerCount();
            }
        }
    }
}
