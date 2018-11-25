using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GodsGame
{
    public class GameManager : Singleton<GameManager>
    {
        #region Public Var
        [HideInInspector]
        public Timer timer;
        #endregion

        #region Private Var
        private string m_CurrentSceneName;
        #endregion

        #region Properties
        public StatsManager StatsManager { get; private set; }
        #endregion

        protected override void OnAwake()
        {
            Debug.Log("GameManager OnAwake");
            if (StatsManager == null)
            {
                ProfileManager.Init();
                ProfileManager.DUMMY_CreateDefaultProfile();
                StatsManager = new StatsManager();
            }
        }

        private void Start()
        {
            Debug.Log("GameManager Start");
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            SceneManager.activeSceneChanged += OnChangeScene;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            SceneManager.activeSceneChanged -= OnChangeScene;
        }

        private void OnChangeScene(Scene current, Scene next)
        {
            //TODO : Only save when current is a playable level => check name ?
            Debug.Log("OnChangeSceneEvent " + current.name + " " + next.name);
            Debug.Log("OnChangeSceneEvent previousScene" + m_CurrentSceneName);
            if (IsScenePlayableLevel(m_CurrentSceneName))
            {
                Debug.Log("Scene is playable");
                timer.Stop();
                StatsManager.ActiveLevelP1.LastStats.TimeCompletion.Count = (float)timer.GetTime.TotalMinutes;
                StatsManager.ActiveLevelP1.CalculateScore();
                StatsManager.ActiveLevelP2.CalculateScore();
                StatsManager.ActiveLevelP1.CalulateTotal();
                StatsManager.ActiveLevelP2.CalulateTotal();
                StatsManager.PlayerProfiles.ForEach((x) => { x.CalculateTotal(); SaveLoad.SaveProfile(x); });
            }
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("FinishLoadingEvent " + scene.name);
            m_CurrentSceneName = scene.name;
            if (IsScenePlayableLevel(scene))
            {
                timer = new Timer(true);
                StatsManager.ChangeLevel(scene.buildIndex);
            }
        }

        public bool IsScenePlayableLevel(Scene scene)
        {
            return scene.name != null && scene.name.Contains("_Level_");
        }

        public bool IsScenePlayableLevel(string sceneName)
        {
            return sceneName != null && sceneName.Contains("_Level_");
        }

        public List<int> GetPlayableLevelBuildIndex()
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = Path.GetFileNameWithoutExtension(path);
                if (IsScenePlayableLevel(sceneName))
                    indexes.Add(i);
            }
            return indexes;
        }
    }
}
