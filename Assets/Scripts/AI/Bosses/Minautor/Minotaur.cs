using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class Minotaur : MonoBehaviour {

        static private string[] BoohSounds = new string[] {
            "arena_crowd_booh",
            "arena_crowd_booh_and_cheer",
        };

        public Animator animator;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void onTakeDamage(Damager damager, Damageable damageable)
        {
            CrowdManager.instance.SetState(CrowdManager.STATES.OOH, 1000);
            AudioManager.Instance.PlaySfx(BoohSounds[Random.Range(0, BoohSounds.Length)], "arena_ambience");
            animator.SetInteger("life", damageable.CurrentHealth);
            animator.SetTrigger("hit");
        }
    }
}