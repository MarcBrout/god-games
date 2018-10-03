/* Speech manager class
 *
 * By: Demis Terborg
 *
 * Create SpeechBubble using SpeechBubbleManager Instance:
 * VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble(transform, "Hello world!");
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public static class SpeechManager
    {
        public static T GetRandomSpeech<T>(T[] speech)
        {
            return speech[UnityEngine.Random.Range(0, speech.Length)];
        }
    }
}