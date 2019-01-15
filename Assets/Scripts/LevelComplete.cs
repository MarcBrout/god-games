using UnityEngine;
using TMPro;
using System;

namespace GodsGame
{
    public class LevelComplete : MonoBehaviour
    {
        public TextMeshProUGUI time;
        public TextMeshProUGUI hitP1;
        public TextMeshProUGUI hitP2;
        public TextMeshProUGUI score;
        public HighScoreScript HighScoreScript;

        void Start()
        {
            
            PlayerLevelStats p1Stat = GameManager.Instance.StatsManager.ActiveLevelP1.LastStats;
            PlayerLevelStats p2Stat = GameManager.Instance.StatsManager.ActiveLevelP2.LastStats;

            TimeSpan ts = TimeSpan.FromMinutes(p1Stat.TimeCompletion.Count);
            time.text +=  string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Minutes, ts.Seconds, ts.Milliseconds);
            hitP1.text += p1Stat.HitTaken.Score.ToString();
            hitP2.text += p2Stat.HitTaken.Score.ToString();
            score.text += (p1Stat.TotalScore + p2Stat.TotalScore).ToString();
            HighScoreScript.SubmitScore(p1Stat.TotalScore + p2Stat.TotalScore);
            
            
            //HighScoreScript.SubmitScore(1500);

        }

    }
}