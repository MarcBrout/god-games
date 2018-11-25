using UnityEngine;
using SpeechBubbleManager = VikingCrewTools.UI.SpeechBubbleManager;

namespace GodsGame
{
    public class PlayerEventReceiver : MonoBehaviour
    {
        public GameObject head;

        private AudioSource _audio;
        private int dashCount = 0;
        private int hitCount = 0;

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
        }

        public void RunStep()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_run", ref _audio);
        }

        public void DashSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_dash", ref _audio);

            if (dashCount >= 3)
            {
                VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble
                    (head.transform, Speech.GetSpeech(EnumAction.PLAYER_DASH, EnumLevel.ANY));
                dashCount = 0;
            }
            else
                ++dashCount;
        }

        public void DeathSound(Damager damager, Damageable damageable)
        {
            CrowdManager.Instance.SetState(SpectatorBehaviour.STATES.PLAYER_HITTEN, 10);
            AudioManager.Instance.PlayRandomSfx3D("player_death", ref _audio);
            AudioManager.Instance.PlaySfx(_deathSounds[Random.Range(0, _deathSounds.Length)], "arena_ambience");

            VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble
                (head.transform, Speech.GetSpeech(EnumAction.PLAYER_DIES, EnumLevel.ANY));
        }

        public void HitSound(Damager damager, Damageable damageable)
        {
            Debug.Log("event_player_hit");
            CrowdManager.Instance.SetState(SpectatorBehaviour.STATES.CHEER, 10);
            AudioManager.Instance.PlaySfx(_cheerSounds[Random.Range(0, _cheerSounds.Length)], "arena_ambience");
            AudioManager.Instance.PlayRandomSfx3D("player_hit", ref _audio);

            if (hitCount >= 3)
            {
                SpeechBubbleManager.Instance.AddSpeechBubble
                    (head.transform, Speech.GetSpeech(EnumAction.PLAYER_TAKESDAMAGE, EnumLevel.ANY));
                hitCount = 0;
            }
            else
                ++hitCount;
        }

        public void AttackSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("items_sword_hit_nothing", ref _audio);
        }
    }
}