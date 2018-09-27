using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class MinotaurEventsReceiver : MonoBehaviour {
        private AudioSource audio;

        // Use this for initialization
        void Start() {
            audio = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void AxeWind()
        {
            AudioManager.Instance.PlayRandomSfx3D("items_sword_hit_nothing", ref audio);
        }
        
        public void WalkStep()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_step_01", "minotaur", ref audio);
        }

        public void Hit()
        {
            AudioManager.Instance.PlayRandomSfx3D("minotaur_hit", ref audio);
        }

        public void Death()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_falling_01", "minotaur", ref audio);
        }

        public void Enrage()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_enrage_01", "minotaur", ref audio);
        }


    }
}