/* Speech class
 *
 * By: Demis Terborg
 *
 * Create SpeechBubble using SpeechBubbleManager Instance:
 * VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble(transform, "Hello world!");
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace GodsGame
{
    public enum EnumAction
    {
        PLAYER_DODGEATTACK
    }

    public enum EnumLevel
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        ANY
    }

    public static class Speech
    {

        public static string GetRandomSpeech(EnumAction a, EnumLevel l)
        {
            List<string> speech = new List<string>();
            int i = 0;

            foreach (var item in ParseJSON.Instance.dialog)
            {
                if (item.Action == Enum.GetName(typeof(EnumAction), a) &&
                    item.Level == Enum.GetName(typeof(EnumLevel), l))
                {
                    speech.Add(item.Speech);
                    i++;
                }
            }

            Random r = new Random();
            int index = r.Next(speech.Count);
            return speech[index];
        }
    }
}