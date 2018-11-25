/* Speech class
 *
 * By: Demis Terborg
 *
 * Create SpeechBubble using SpeechBubbleManager Instance:
 * VikingCrewTools.UI.SpeechBubbleManager.Instance.AddSpeechBubble(transform, GetRandomSpeech(EnumAction a, EnumLevel l);
 */

using System;
using System.Collections.Generic;
using Random = System.Random;

namespace GodsGame
{
    public enum EnumAction
    {
        CROWD_DISAPROVAL,
        CROWD_PLAYER_HITTEN,
        MINOTAUR_AXEWIND,
        MINOTAUR_DIES,
        MINOTAUR_DURINGFIGHT,
        MINOTAUR_TAKESDAMAGE,
        PLAYER_AFTERBETRAYAL,
        PLAYER_BEFOREBETRAYAL,
        PLAYER_DAMAGESBOSS,
        PLAYER_DASH,
        PLAYER_DIES,
        PLAYER_DURINGFIGHT,
        PLAYER_ITEMPICKUP,
        PLAYER_OTHERTAKESDAMAGE,
        PLAYER_TAKESDAMAGE,
        PLAYER_THROWWEAPON,
        PLAYER_WIN
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
        public static string GetSpeech(EnumAction a, EnumLevel l)
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