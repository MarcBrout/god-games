using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GodsGame
{
    public class PauseManager : MonoBehaviour
    {
        public AudioMixerSnapshot paused;
        public AudioMixerSnapshot unpaused;

        private Canvas canvas;

        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                canvas.enabled = !canvas.enabled;
                PauseMusic();
            }
        }

        public void PauseMusic()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        void LowPass()
        {
            if (Time.timeScale == 0)
            {
                paused.TransitionTo(.01f);
            }

            else
            {
                unpaused.TransitionTo(.01f);
            }
        }
    }
}
