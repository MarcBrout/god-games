using UnityEngine;
using System.Collections;
using SpeechBubbleManager = VikingCrewTools.UI.SpeechBubbleManager;

namespace GodsGame
{
    [RequireComponent(typeof(Animator))]
    public class PlayerEventReceiver : MonoBehaviour
    {
        public GameObject _head;
        public GameObject _electricMesh;
        public float _disableElectricMeshAfter;

        private AudioSource _audio;
        private int _dashCount = 0;
        private int _hitCount = 0;
        private Animator _animator;
        private readonly int _SwordPickUp = Animator.StringToHash("HasItem");
        private readonly int _UseSword = Animator.StringToHash("UseSword");

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
            _animator = GetComponent<Animator>();
            _audio = GetComponent<AudioSource>();
        }

        public void RunStep()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_run", ref _audio);
        }

        public void DashSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("player_dash", ref _audio);

            if (_dashCount >= 3)
            {
                VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble
                    (_head.transform, Speech.GetSpeech(EnumAction.PLAYER_DASH, EnumLevel.ANY));
                _dashCount = 0;
            }
            else
                ++_dashCount;
        }

        public void DeathSound(Damager damager, Damageable damageable)
        {
            CrowdManager.Instance.SetState(SpectatorBehaviour.STATES.PLAYER_HITTEN, 10);
            AudioManager.Instance.PlayRandomSfx3D("player_death", ref _audio);
            AudioManager.Instance.PlaySfx(_deathSounds[Random.Range(0, _deathSounds.Length)], "arena_ambience");

            VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble
                (_head.transform, Speech.GetSpeech(EnumAction.PLAYER_DIES, EnumLevel.ANY));
        }

        public void HitSound(Damager damager, Damageable damageable)
        {
            Debug.Log("event_player_hit");
            CrowdManager.Instance.SetState(SpectatorBehaviour.STATES.CHEER, 10);
            AudioManager.Instance.PlaySfx(_cheerSounds[Random.Range(0, _cheerSounds.Length)], "arena_ambience");
            AudioManager.Instance.PlayRandomSfx3D("player_hit", ref _audio);

            if (damager.gameObject.layer == LayerMask.NameToLayer("Electricity"))
            {
                _electricMesh.SetActive(true);
                StartCoroutine(DisableEletricMesh());
            }

            if (_hitCount >= 3)
            {
                SpeechBubbleManager.Instance.AddSpeechBubble
                    (_head.transform, Speech.GetSpeech(EnumAction.PLAYER_TAKESDAMAGE, EnumLevel.ANY));
                _hitCount = 0;
            }
            else
                ++_hitCount;
        }

        IEnumerator DisableEletricMesh()
        {
            yield return new WaitForSeconds(_disableElectricMeshAfter);
            _electricMesh.SetActive(false);
        }

        public void AttackSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("items_sword_hit_nothing", ref _audio);
        }

        public void OnSwordPickUp()
        {
            _animator.SetFloat(_SwordPickUp, 1f);
        }

        public void OnSwordDrop()
        {
            _animator.SetFloat(_SwordPickUp, 0f);
        }

        public void OnSwordAttack()
        {
            _animator.SetTrigger(_UseSword);
        }
    }
}