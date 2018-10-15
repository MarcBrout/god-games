using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechBubbleManager = VikingCrewTools.UI.SpeechBubbleManager;

namespace GodsGame
{
    public class PlayerEventReceiver : MonoBehaviour
    {
        public GameObject head;

        private AudioSource _audio;
        private Animator _animator;
        static private string[] _deathSounds = new string[] {
            "arena_crowd_laugh_01",
            "arena_crowd_laugh_02",
            "arena_crowd_laugh_03",
            "arena_crowd_ouch_02",
        };

        static private string[] _cheerSounds = new string[] {
            "arena_crowd_claps_and_cheers_01",
            "arena_crowd_claps_and_cheers_02",
            "arena_crowd_claps_and_cheers_03",
            "arena_crowd_ouch_01",
        };

        void Start()
        {
            _audio = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }

        public void RunStep()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_run", ref _audio);
        }

        public void DashSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_dash", ref _audio);

            VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble
                (head.transform, Speech.GetSpeech(EnumAction.PLAYER_DASH, EnumLevel.ANY));
        }

        public void DeathSound(Damager damager, Damageable damageable)
        {
            CrowdManager.instance.SetState(CrowdManager.STATES.OOH, 1000);
            AudioManager.Instance.PlayRandomSfx3D("player_death", ref _audio);
            AudioManager.Instance.PlaySfx(_deathSounds[Random.Range(0, _deathSounds.Length)], "arena_ambience");
            _animator.SetTrigger("Died");

            VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble
                (head.transform, Speech.GetSpeech(EnumAction.PLAYER_DIES, EnumLevel.ANY));
        }

        public void HitSound(Damager damager, Damageable damageable)
        {
            //CrowdManager.instance.SetState(CrowdManager.STATES.CHEER, 1000);
            AudioManager.Instance.PlaySfx(_cheerSounds[Random.Range(0, _cheerSounds.Length)], "arena_ambience");
            AudioManager.Instance.PlayRandomSfx3D("player_hit", ref _audio);

            SpeechBubbleManager.Instance.AddSpeechBubble
                (head.transform, Speech.GetSpeech(EnumAction.PLAYER_TAKESDAMAGE, EnumLevel.ANY));
        }
    }
}