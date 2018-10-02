using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public static class SpeechEventManager
    {
        public static T GetRandomSpeech<T>(T[] speech)
        {
            return speech[UnityEngine.Random.Range(0, speech.Length)];
        }
    }
}