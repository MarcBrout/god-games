using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class CyclopAnimationScript : MonoBehaviour
    {

        private AudioSource _audioSource;

        // Use this for initialization
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        public void ThrowRockSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("cyclop_throw", ref _audioSource);
        }
    }
}