using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;
using SpeechBubbleManager = VikingCrewTools.UI.SpeechBubbleManager;

namespace GodsGame
{
    public class MinotaurEventsReceiver : MonoBehaviour
    {
        private AudioSource audio;
        private int axeWindCount = 0;

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        void Update()
        {

        }

        public void AxeWind()
        {
            if (axeWindCount == 5)
            {
                SpeechBubbleManager.Instance.AddSpeechBubble
                    (transform, Speech.GetSpeech(EnumAction.MINOTAUR_AXEWIND, EnumLevel.ANY),
                    SpeechBubbleManager.SpeechbubbleType.ANGRY);
                axeWindCount = 0;
            }

            ++axeWindCount;

            AudioManager.Instance.PlayRandomSfx3D("items_sword_hit_nothing", ref audio);
        }

        public void WalkStep()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_step_01", "minotaur", ref audio);
        }

        public void Hit()
        {
            AudioManager.Instance.PlayRandomSfx3D("minotaur_hit", ref audio);
        }

        public void Death()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_falling_01", "minotaur", ref audio);
        }

        public void Enrage()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_enrage_01", "minotaur", ref audio);
        }


    }
}