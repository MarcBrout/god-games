using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GodsGame
{
    //TODO : remove this and link profile to player object 
    public class StatsManager
    {
        #region Properties
        public List<PlayerProfile> PlayerProfiles { get; private set; }
        public PlayerLevelProfile ActiveLevelP1 { get; private set; }
        public PlayerLevelProfile ActiveLevelP2 { get; private set; }
        #endregion

        #region Private Var
        private readonly string m_Player1Name = "Player_1";
        private readonly string m_Player2Name = "Player_2";
        #endregion

        public StatsManager()
        {
            PlayerProfiles = ProfileManager.LoadedProfile;
        }

        public void ChangeLevel(int sceneIndex)
        {
            if (PlayerProfiles.Count >= 2)
            {
                ActiveLevelP1 = GetActiveLevelStat(PlayerProfiles[0], sceneIndex);
                ActiveLevelP2 = GetActiveLevelStat(PlayerProfiles[1], sceneIndex);
            }
        }

        private PlayerLevelProfile GetActiveLevelStat(PlayerProfile profile, int sceneIndex)
        {
            return profile.LevelsStats.Find(x => x.LevelBuildIndex == sceneIndex);
        }

        //TODO: Change all the following function
        public void SetHeathCountFor(string player, int health)
        {
            if (player == m_Player1Name)
                SetHealthCount_P1(health);
            else if (player == m_Player2Name)
                SetHealthCount_P2(health);
            else
                Debug.LogWarning("SetHealthFor: Player " + player + " doesn't exist");
        }

        public void AddDodgeCountFor(string player)
        {
            if (player == m_Player1Name)
                AddDodgeCount_P1();
            else if (player == m_Player2Name)
                AddDodgeCount_P2();
            else
                Debug.LogWarning("AddDodgeCountFor: Player " + player + " doesn't exist");
        }

        public void AddDeathCountFor(string player)
        {
            if (player == m_Player1Name)
                AddDeathCount_P1();
            else if (player == m_Player2Name)
                AddDeathCount_P2();
            else
                Debug.LogWarning("AddDeathCountFor: Player " + player + " doesn't exist");
        }

        public void AddHitCountFor(string player)
        {
            if (player == m_Player1Name)
                AddHitCount_P1();
            else if (player == m_Player2Name)
                AddHitCount_P2();
            else
                Debug.LogWarning("AddHitCountFor: Player " + player + " doesn't exist");
        }

        private void SetHealthCount_P1(int health)
        {
            ActiveLevelP1.LastStats.HealthRemaining.Count = health;
        }

        private void SetHealthCount_P2(int health)
        {
            ActiveLevelP2.LastStats.HealthRemaining.Count = health;
        }

        private void AddDodgeCount_P1()
        {
            ++(ActiveLevelP1.LastStats.DodgeMade.Count);
        }

        private void AddDodgeCount_P2()
        {
            ++(ActiveLevelP2.LastStats.DodgeMade.Count);
        }

        private void AddDeathCount_P1()
        {
            ++(ActiveLevelP1.LastStats.Death.Count);
        }

        private void AddDeathCount_P2()
        {
            ++(ActiveLevelP2.LastStats.Death.Count);
        }

        private void AddHitCount_P1()
        {
            ++(ActiveLevelP1.LastStats.HitTaken.Count);
        }

        private void AddHitCount_P2()
        {
            ++(ActiveLevelP2.LastStats.HitTaken.Count);
        }
    }
}
