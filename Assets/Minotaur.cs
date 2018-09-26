using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class Minotaur : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void onTakeDamage(Damager damager, Damageable damageable)
        {
            Debug.Log("I GOT HIT ARG");
        }
    }
}