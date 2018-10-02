using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class PlayerEventReceiver : MonoBehaviour
    {
        private AudioSource audio;
        static private string[] DeathSounds = new string[] {
            "arena_crowd_laugh_01",
            "arena_crowd_laugh_02",
            "arena_crowd_laugh_03",
            "arena_crowd_ouch_02",
        };

        void Start()
        {
            audio = GetComponent<AudioSource>();
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
            CrowdManager.instance.SetState(CrowdManager.STATES.CHEER, 1000);
            AudioManager.Instance.PlayRandomSfx3D("player_death", ref audio);
            AudioManager.Instance.PlaySfx(DeathSounds[Random.Range(0, DeathSounds.Length)], "arena_ambience");
        }

        public void Hit(Damager damager, Damageable damageable)
        {
            AudioManager.Instance.PlayRandomSfx3D("player_hit", ref audio);
        }
    }
}