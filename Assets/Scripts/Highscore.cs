using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    [Serializable]
    public class Highscore {

        [SerializeField]
        public KeyValuePair<string, float>[] HighscoreArr { get; set; }
        [SerializeField]
        public int Level { get; private set; }

        public Highscore(int level) {
            this.Level = level;
            this.HighscoreArr = new KeyValuePair<string, float>[3];
        }

        public void UpdateHighScores(string name, float score) {
            KeyValuePair<string, float> tmpScore = new KeyValuePair<string, float>(name, score);

            for (int i = 0; i < HighscoreArr.Length; i++) {

                if (HighscoreArr[i].Equals(null))
                {
                    HighscoreArr[i] = tmpScore;
                }
                else if (HighscoreArr[i].Value < score) {
                    KeyValuePair<string, float> s = HighscoreArr[i];
                    HighscoreArr[i] = tmpScore;
                    tmpScore = s;
                }
            }
        }

        public int CheckForNewHighscore(float score) {
            for (int i = 0; i < HighscoreArr.Length; i++)
            {

                if (HighscoreArr[i].Equals(null))
                {
                    return i;
                }
                else if (HighscoreArr[i].Value < score)
                {
                    return i;
                }
            }
            return -1;
        }
    }

   
}