﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame {
    public class PlayerEventReceiver : MonoBehaviour {
        private AudioSource audio;

        // Use this for initialization
        void Start() {
            audio = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void RunStep()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_run", ref audio);
        }

        public void Dash()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_dash", ref audio);
        }

        public void Death(Damager damager, Damageable damageable)
        {
            AudioManager.Instance.PlayRandomSfx3D("player_death", ref audio);
        }

        public void Hit(Damager damager, Damageable damageable)
        {
            AudioManager.Instance.PlayRandomSfx3D("player_hit", ref audio);
        }
    }
}