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

        static private string[] CheerSounds = new string[] {
            "arena_crowd_claps_and_cheers_01",
            "arena_crowd_claps_and_cheers_02",
            "arena_crowd_claps_and_cheers_03",
            "arena_crowd_ouch_01",
        };

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void RunStep()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_run", ref audio);
        }

        public void DashSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_dash", ref audio);
        }

        public void DeathSound(Damager damager, Damageable damageable)
        {
            CrowdManager.instance.SetState(CrowdManager.STATES.OOH, 1000);
            AudioManager.Instance.PlayRandomSfx3D("player_death", ref audio);
            AudioManager.Instance.PlaySfx(DeathSounds[Random.Range(0, DeathSounds.Length)], "arena_ambience");
        }

        public void HitSound(Damager damager, Damageable damageable)
        {
            Debug.Log("HIT");
            //CrowdManager.instance.SetState(CrowdManager.STATES.CHEER, 1000);
            AudioManager.Instance.PlaySfx(CheerSounds[Random.Range(0, CheerSounds.Length)], "arena_ambience");
            AudioManager.Instance.PlayRandomSfx3D("player_hit", ref audio);
        }
    }
}