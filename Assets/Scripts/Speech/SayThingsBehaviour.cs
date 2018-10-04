using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechBubbleManager = VikingCrewTools.UI.SpeechBubbleManager;

namespace GodsGame
{
    public class SayThingsBehaviour : MonoBehaviour
    {

        public Transform head;
        public float timeBetweenSpeak = 30f;
        public bool doTalkOnYourOwn = true;
        private float timeToNextSpeak;

        void Start()
        {
            timeToNextSpeak = timeBetweenSpeak;
        }

        void Update()
        {
            timeToNextSpeak -= Time.deltaTime;

            if (doTalkOnYourOwn && timeToNextSpeak <= 0)
                SaySomething();
        }

        public void SaySomething()
        {
            if (transform.CompareTag("Player"))
            {
                string message = Speech.GetSpeech(EnumAction.PLAYER_DURINGFIGHT, EnumLevel.ANY);
                SaySomething(message, SpeechBubbleManager.SpeechbubbleType.SERIOUS);
            }
            else
            {
                string message = Speech.GetSpeech(EnumAction.MINOTAUR_DURINGFIGHT, EnumLevel.ANY);
                SaySomething(message, SpeechBubbleManager.SpeechbubbleType.ANGRY);
            }
        }

        //public void SaySomething(string message)
        //{
        //    SaySomething(message, SpeechBubbleManager.Instance.GetRandomSpeechbubbleType());
        //}

        public void SaySomething(string message, SpeechBubbleManager.SpeechbubbleType speechbubbleType)
        {
            if (head == null)
                SpeechBubbleManager.Instance.AddSpeechBubble(transform, message, speechbubbleType);
            else
                SpeechBubbleManager.Instance.AddSpeechBubble(head, message, speechbubbleType);

            timeToNextSpeak = timeBetweenSpeak;
        }
    }
}

