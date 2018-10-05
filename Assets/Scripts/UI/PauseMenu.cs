using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace GodsGame
{

    public class PauseMenu : Singleton<PauseMenu>
    {

        public bool GamesIsPaused = false;
        public GameObject pauseMenuUI;

        private readonly string menu = "MainMenuScene";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GamesIsPaused)
                    Resume();
                else
                    Pause();
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GamesIsPaused = false;
        }

        public void LoadControlMenu()
        {
            Debug.Log("LoadControlMenu");
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(menu);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GamesIsPaused = true;
        }
    }
}
