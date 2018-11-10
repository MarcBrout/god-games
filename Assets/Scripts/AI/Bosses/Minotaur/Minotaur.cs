using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class Minotaur : MonoBehaviour {

        public CrowdManager crowdManager;

        static private string[] BoohSounds = new string[] {
            "arena_crowd_booh",
            "arena_crowd_booh_and_cheer",
        };

        public Animator animator;

        public void OnTakeDamage(Damager damager, Damageable damageable)
        {
            Debug.Log("BOSS IS HIT !");
            crowdManager.SetState(CrowdManager.STATES.OOH, 1000);
            AudioManager.Instance.PlaySfx(BoohSounds[Random.Range(0, BoohSounds.Length)], "arena_ambience");
            animator.SetInteger("life", damageable.CurrentHealth);
            animator.SetTrigger("hit");
        }
    }
}