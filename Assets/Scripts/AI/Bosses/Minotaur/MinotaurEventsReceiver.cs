﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;
using SpeechBubbleManager = VikingCrewTools.UI.SpeechBubbleManager;

namespace GodsGame
{
    public class MinotaurEventsReceiver : MonoBehaviour
    {
        public GameObject head;

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
            AudioManager.Instance.PlayRandomSfx3D("items_sword_hit_nothing", ref audio);

            if (axeWindCount == 5)
            {
                SpeechBubbleManager.Instance.AddSpeechBubble
                (head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_AXEWIND, EnumLevel.ANY),
                    SpeechBubbleManager.SpeechbubbleType.ANGRY);
                axeWindCount = 0;
            }
            else
            {
                ++axeWindCount;
            }
        }

        public void WalkStep()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_step_01", "minotaur_step", ref audio);
        }

        public void Hit()
        {
            AudioManager.Instance.PlayRandomSfx3D("minotaur_pain", ref audio);

            SpeechBubbleManager.Instance.AddSpeechBubble
            (head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_TAKESDAMAGE, EnumLevel.ANY),
                SpeechBubbleManager.SpeechbubbleType.ANGRY);
        }

        public void Death()
        {
            //AudioManager.Instance.PlaySfx3D("minotaur_falling_01", "minotaur", ref audio);

            AudioManager.Instance.PlayRandomSfx3D("minotaur_falling", ref audio);

            SpeechBubbleManager.Instance.AddSpeechBubble
            (head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_DIES, EnumLevel.ANY),
                SpeechBubbleManager.SpeechbubbleType.ANGRY);
        }

        public void Enrage()
        {
            AudioManager.Instance.PlaySfx3D("minotaur_enrage_01", "minotaur_enrage", ref audio);

            //todo: add enumAction MINOTAUR_ENRAGE
            //SpeechBubbleManager.Instance.AddSpeechBubble
            //(head.transform, Speech.GetSpeech(EnumAction.MINOTAUR_TAKESDAMAGE, EnumLevel.ANY),
            //    SpeechBubbleManager.SpeechbubbleType.ANGRY);
        }
    }
}