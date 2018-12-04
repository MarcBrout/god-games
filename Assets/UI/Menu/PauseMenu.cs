using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace GodsGame
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        [HideInInspector]
        public bool GamesIsPaused = false;
        public GameObject pauseMenuUI;

        private readonly string m_Menu = "MainMenuArena";
        private ChangeControlMenu m_ControlMenu;

        private void Start()
        {
            m_ControlMenu = GameObject.FindGameObjectWithTag("ControlMenu").GetComponent<ChangeControlMenu>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GamesIsPaused)
                    if (m_ControlMenu.ControlMenuIsActive())
                    {
                        m_ControlMenu.GoBack();
                    }
                    else
                    {
                        Resume();
                    }
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
            pauseMenuUI.SetActive(false);
            m_ControlMenu.controlMenuUI.SetActive(true);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(m_Menu);
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
