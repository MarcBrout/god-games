using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class Minotaur : MonoBehaviour {

        public Animator animator;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void onTakeDamage(Damager damager, Damageable damageable)
        {
            animator.SetInteger("life", damageable.CurrentHealth);
            animator.SetTrigger("hit");
        }
    }
}