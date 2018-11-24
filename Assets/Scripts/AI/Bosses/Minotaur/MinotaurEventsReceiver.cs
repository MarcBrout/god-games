using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;
using SpeechBubbleManager = VikingCrewTools.UI.SpeechBubbleManager;

namespace GodsGame
{
    public class MinotaurEventsReceiver : MonoBehaviour
    {
        public GameObject head;
        public ParticleSystem _AxeSlash;

        private AudioSource _audio;
        private int axeWindCount = 0;
        private Collider _axeCollider;
       

        void Start()
        {
            _audio = GetComponent<AudioSource>();
            _axeCollider = GetComponentInChildren<Collider>();
        }

        void Update()
        {

        }

        public void AxeWind()
        {
            Debug.Log("minotaur_event_axewind");

            if (_AxeSlash)
            {
                _AxeSlash.Play();
            }

            AudioManager.Instance.PlayRandomSfx3D("items_sword_hit_nothing", ref _audio);

            if (axeWindCount >= 5)
            {
                SpeechBubbleManager.Instance.AddSpeechBubble
                (head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_AXEWIND, EnumLevel.ANY),
                    SpeechBubbleManager.SpeechbubbleType.ANGRY);
                axeWindCount = 0;
            }
            else
                ++axeWindCount;
        }

        public void WalkStep()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_step_01", "minotaur_step", ref _audio);
        }

        public void Hit()
        {
            Debug.Log("minotaur_event_hit");

            AudioManager.Instance.PlayRandomSfx3D("minotaur_pain", ref _audio);

            SpeechBubbleManager.Instance.AddSpeechBubble
            (head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_TAKESDAMAGE, EnumLevel.ANY),
                SpeechBubbleManager.SpeechbubbleType.ANGRY);
        }

        public void Death()
        {
            Debug.Log("minotaur_event_death");

            //AudioManager.Instance.PlaySfx3D("minotaur_falling_01", "minotaur", ref _audio);

            AudioManager.Instance.PlayRandomSfx3D("minotaur_falling", ref _audio);

            SpeechBubbleManager.Instance.AddSpeechBubble
            (head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_DIES, EnumLevel.ANY),
                SpeechBubbleManager.SpeechbubbleType.ANGRY);
        }

        public void Enrage()
        {
            Debug.Log("minotaur_event_enrage");

            AudioManager.Instance.PlaySfx3D("minotaur_enrage_01", "minotaur_enrage", ref _audio);

            //todo: add enumAction MINOTAUR_ENRAGE
            //SpeechBubbleManager.Instance.AddSpeechBubble
            //(head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_TAKESDAMAGE, EnumLevel.ANY),
            //    SpeechBubbleManager.SpeechbubbleType.ANGRY);
        }

        public void ActivateAxe()
        {
            _axeCollider.enabled = true;
        }

        public void DeactivateAxe()
        {
            _axeCollider.enabled = false;
        }
    }
}