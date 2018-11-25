using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GodsGame
{
    //TODO remove this and link profile to player object
    public static class ProfileManager
    {
        public static List<PlayerProfile> LoadedProfile { get; private set; }
        public static PlayerProfile IdealScoreProfile { get; private set; }

        public static void Init()
        {
            LoadedProfile = new List<PlayerProfile>();
            CreateIdealProfile();
        }

        public static List<PlayerProfile> DUMMY_CreateDefaultProfile()
        { 
            string profile_1 = "PlayerProfile_1";
            string profile_2 = "PlayerProfile_2";

            LoadedProfile.AddIfNotNull(CreateProfile(profile_1));
            LoadedProfile.AddIfNotNull(CreateProfile(profile_2));
            return LoadedProfile;
        }

        public static PlayerProfile CreateProfile(string profileName, bool save = true)
        {
            PlayerProfile profile = SaveLoad.LoadProfile(profileName);
            if (profile == null)
            {
                List<int> levelsIndex = GameManager.Instance.GetPlayableLevelBuildIndex();
                List<PlayerLevelProfile> levelsStats = levelsIndex.Select(x => new PlayerLevelProfile() {
                    LevelBuildIndex = x
                }).ToList();
                profile = new PlayerProfile(profileName, levelsStats);
                if (save)
                    SaveLoad.SaveProfile(profile);
            }
            return profile;
        }

        public static PlayerLevelProfile GetIdealScoreForLevel(PlayerLevelProfile playerLevelProfile)
        {
            return IdealScoreProfile
                .LevelsStats
                .Find(x => x.LevelBuildIndex == playerLevelProfile.LevelBuildIndex);
        }

        private static void CreateIdealProfile()
        {
            string idealProfileName = "IdealScoreProfile";
            if (!SaveLoad.DoesProfileExist(idealProfileName))
            {
                IdealScoreProfile = CreateProfile(idealProfileName, false);
                IdealScoreProfile.LevelsStats.ForEach(x =>
                {
                    Debug.Log("ideal score sceneBuildIndex " + x.LevelBuildIndex);
                    x.BestStats.HealthRemaining.Count = 3; //get max player health
                    x.BestStats.DodgeMade.Count = 0; //no need to dodge... Adapt this for each level ?
                    x.BestStats.HitTaken.Count = 0; //never hitten...
                    x.BestStats.Death.Count = 0; //never died...
                    x.BestStats.TimeCompletion.Count = (float)TimeSpan.FromMinutes(10).TotalMinutes; // X minutes => adapt this BesTimeCompletion for each level
                    x.CalculateScore();
                    x.LastStats = x.TotalStats = x.BestStats;
                });
                SaveLoad.SaveProfile(IdealScoreProfile);
            }
            else
                IdealScoreProfile = SaveLoad.LoadProfile(idealProfileName);
        }
    }
}

