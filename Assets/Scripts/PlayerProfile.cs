using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GodsGame
{
    public static class ScoreConstant
    {

        //TODO: test and tweak value
        public static readonly float timeScoreBase = 5000;
        public static readonly float hitScoreBase = -1000;
        public static readonly float healthScoreBase = 1000;
        public static readonly float dodgeScoreBase = 1000;
        public static readonly float deathScoreBase = 1000;

        //TODO: text and tweak value
        //percentage of decrease => value between 0 and 1
        public static readonly float timeScorePercentDecrease = 0.9f;
        public static readonly float hitScoreBPercentDecrease = 0.5f;
        public static readonly float dodgeScorePercentDecrease = 0.9f;
        public static readonly float healthScoreBPercentDecrease = 0.9f;
        public static readonly float deathScorePercentDecrease = 0.9f;
    }

    [Serializable]
    public class ScorePairValue
    {
        #region Private Var
        [SerializeField]
        private float m_Count;
        [SerializeField]
        private long m_Score;
        #endregion

        #region Properties
        public float Count { get { return m_Count; } set { m_Count = value; } }
        public long Score { get { return m_Score; } set { m_Score = value; } }
        #endregion

        public ScorePairValue(float count = 0, long score = 0)
        {
            m_Count = count;
            m_Score = score;
        }

        /// <summary>
        /// Calculate score based on the formula score(value) = S * pow(0.9, value - N); 
        /// Meaning N+1 value yield a score of S * 0.9 (ie. 90% of the best score), then N+2 value yield S * 0.81 (ie. 90% of 90% of the best score), and so on
        /// If N > value we'll have a big score
        /// If N == value we'll have a score of S
        /// </summary>
        /// <param name="scoreBase"></param>
        /// <param name="percentScoreDecrease">Value between 0 and 1</param>
        /// <param name="idealScore"></param>
        public void CalculatePositiveScore(float scoreBase, float percentScoreDecrease, float idealScore)
        {
            Score = (long)(scoreBase * Mathf.Pow(percentScoreDecrease, m_Count - idealScore));
        }

        /// <summary>
        /// Calculate score based on the formula score(value) = S * pow(0.9, N - value) - S; 
        /// Meaning N+1 value yield a score of S * 0.9 (ie. 90% of the best score), then N+2 value yield S * 0.81 (ie. 90% of 90% of the best score), and so on.
        /// If N > value we'll have a low score.
        /// If N == value we'll have a score of 0.
        /// </summary>
        /// <param name="scoreBase"></param>
        /// <param name="percentScoreDecrease">Value between 0 and 1</param>
        /// <param name="idealScore"></param>
        public void CalculateNegativeScore(float scoreBase, float percentScoreDecrease, float idealScore)
        {
            Score = (int)(scoreBase * Mathf.Pow(percentScoreDecrease, idealScore - m_Count) - scoreBase);
        }

        public static ScorePairValue operator +(ScorePairValue a, ScorePairValue b)
        {
            return new ScorePairValue(a.Count + b.Count);
        }
    }

    [Serializable]
    public class PlayerLevelStats
    {
        #region Private Var
        [SerializeField]
        private ScorePairValue m_HealthRemaining;
        [SerializeField]
        private ScorePairValue m_HitTaken;
        [SerializeField]
        private ScorePairValue m_DodgeMade;
        [SerializeField]
        private ScorePairValue m_Deatht;
        [SerializeField]
        private ScorePairValue m_TimeCompletion; // in minutes
        [SerializeField]
        private long m_TotalScore;
        #endregion

        #region Properties
        public ScorePairValue HealthRemaining { get { return m_HealthRemaining; } set { m_HealthRemaining = value; } }
        public ScorePairValue HitTaken { get { return m_HitTaken; } set { m_HitTaken = value; } }
        public ScorePairValue DodgeMade { get { return m_DodgeMade; } set { m_DodgeMade = value; } }
        public ScorePairValue Death { get { return m_Deatht; } set { m_Deatht = value; } }
        public ScorePairValue TimeCompletion { get { return m_TimeCompletion; } set { m_TimeCompletion = value; } }
        public long TotalScore { get { return m_TotalScore; } set { m_TotalScore = value; } }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="healthCount"></param>
        /// <param name="hitCount"></param>
        /// <param name="dodgeCount"></param>
        /// <param name="deathCount"></param>
        /// <param name="timeCompletion"></param>
        public PlayerLevelStats(int healthCount = 0, int hitCount = 0, int dodgeCount = 0, int deathCount = 0, int timeCompletion = 0)
        {
            HealthRemaining = new ScorePairValue(healthCount);
            HitTaken = new ScorePairValue(hitCount);
            DodgeMade = new ScorePairValue(dodgeCount);
            Death = new ScorePairValue(deathCount);
            TimeCompletion = new ScorePairValue(timeCompletion);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="healthCount"></param>
        /// <param name="hitCount"></param>
        /// <param name="dodgeCount"></param>
        /// <param name="deathCount"></param>
        /// <param name="timeCompletion"></param>
        public PlayerLevelStats(ScorePairValue healthCount, ScorePairValue hitCount, ScorePairValue dodgeCount, ScorePairValue deathCount, ScorePairValue timeCompletion)
        {
            HealthRemaining = healthCount;
            HitTaken = hitCount;
            DodgeMade = dodgeCount;
            Death = deathCount;
            TimeCompletion = timeCompletion;
        }

        /// <summary>
        /// Calculate the total score for the level
        /// </summary>
        public void CalculateTotalScore(PlayerLevelStats idealScores)
        {
            TimeCompletion.CalculatePositiveScore(ScoreConstant.timeScoreBase, ScoreConstant.timeScorePercentDecrease, idealScores.TimeCompletion.Count);
            HitTaken.CalculateNegativeScore(ScoreConstant.timeScoreBase, ScoreConstant.timeScorePercentDecrease, idealScores.HitTaken.Count);
            TotalScore =  TimeCompletion.Score + HitTaken.Score;
        }

        public static PlayerLevelStats operator +(PlayerLevelStats a, PlayerLevelStats b)
        {
            return new PlayerLevelStats(a.HealthRemaining + b.HealthRemaining,
                a.HitTaken + b.HitTaken, a.DodgeMade + b.DodgeMade, a.Death + b.Death,
               a.TimeCompletion + b.TimeCompletion);
        }

        public static bool operator <(PlayerLevelStats a, PlayerLevelStats b)
        {
            return a.TotalScore < b.TotalScore;
        }

        public static bool operator >(PlayerLevelStats a, PlayerLevelStats b)
        {
            return a.TotalScore > b.TotalScore;
        }
    }

    [Serializable]
    public class PlayerLevelProfile
    {
        #region Private Var
        [SerializeField]
        private int m_LevelBuildIndex;
        [SerializeField]
        private PlayerLevelStats m_TotalStats;
        [SerializeField]
        private PlayerLevelStats m_BestStats;
        [SerializeField]
        private PlayerLevelStats m_LastStats;
        #endregion

        #region Properties
        public int LevelBuildIndex { get { return m_LevelBuildIndex; } set { m_LevelBuildIndex = value; } }
        public PlayerLevelStats TotalStats { get { return m_TotalStats; } set { m_TotalStats = value; } }
        public PlayerLevelStats BestStats { get { return m_BestStats; } set { m_BestStats = value; } }
        public PlayerLevelStats LastStats { get { return m_LastStats; } set { m_LastStats = value; } }
        #endregion;

        public PlayerLevelProfile(int levelBuildIndex = -1, PlayerLevelStats totalStats = null)
        {
            LevelBuildIndex = LevelBuildIndex;
            TotalStats = totalStats ?? new PlayerLevelStats();
            BestStats = new PlayerLevelStats();
            LastStats = new PlayerLevelStats();
        }

        public void CalculateScore()
        {
            PlayerLevelProfile idealLevelScore = ProfileManager.GetIdealScoreForLevel(this);

            LastStats.CalculateTotalScore(idealLevelScore.BestStats);

            //var dodgeScore = m_DodgeScoreBase * Mathf.Pow(m_DodgeScorePercentDecrease, playerLevelProfile.LastStats.DodgeCount - idealLevelScore.BestStats.DodgeCount);
            //var healthScore = m_HealthScoreBase * Mathf.Pow(m_HealthScoreBPercentDecrease, idealLevelScore.BestStats.HealthCount - playerLevelProfile.LastStats.HealthCount);
            //var deathScore = m_DeathScoreBase * Mathf.Pow(m_HealthScoreBPercentDecrease, playerLevelProfile.LastStats.DeathCount - idealLevelScore.BestStats.DeathCount);

            Debug.Log("TimeScore: " + LastStats.TimeCompletion.Score);
            Debug.Log("HitScore: " + LastStats.HitTaken.Score);
            Debug.Log("Total: " + LastStats.TotalScore);
        }

        public void CalulateTotal()
        {
            TotalStats += LastStats;
        }

        private void UpdateBesStats()
        {
            m_BestStats = m_LastStats < m_BestStats ? m_BestStats : m_LastStats;
        }
    }

    [Serializable]
    public class PlayerProfile
    {
        #region Private Var
        [SerializeField]
        private string m_Name;
        [SerializeField]
        private PlayerLevelStats m_TotalStats;
        [SerializeField]
        List<PlayerLevelProfile> m_LevelsStats;
        #endregion

        #region Properties
        public string Name { get { return m_Name; } set { m_Name = value; } }
        public PlayerLevelStats TotalStats { get { return m_TotalStats; } set { m_TotalStats = value; } }
        public List<PlayerLevelProfile> LevelsStats { get { return m_LevelsStats; } set { m_LevelsStats = value; } }
        #endregion;

        public PlayerProfile(string name = "", List<PlayerLevelProfile> levelsStats = null)
        {
            Name = name;
            TotalStats = new PlayerLevelStats();
            LevelsStats = levelsStats ?? new List<PlayerLevelProfile>();
        }

        public void CalculateTotal()
        {
            for (int i = 0; i < LevelsStats.Count; ++i)
                TotalStats += LevelsStats[i].TotalStats;
            //TotalStats = Level_1_Stats.TotalStats + Level_2_Stats.TotalStats + Level_3_Stats.TotalStats;
        }
    }
}
