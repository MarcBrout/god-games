using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class AudioManager : MonoBehaviour
    {

        public static AudioManager instance;

        public AudioMixerGroup mixerGroup;

        public float timeToFade, threshold;

        public Sound[] animation, arena_ambience, arena_battle_music
            , arena_events, items_common, items_sword, minotaur, player_dash
            , player_death, player_hit, player_run, player_walk, zeus;

        private Sound backgroundMusic1, backgroundMusic2, sfx;

        private bool backgroundMusic1IsPlaying;


        // ************************ PUBLIC METHODS ************************ //

        public void PlaySfx(string sound, string soundArr)
        {
            sfx = Array.Find(GetSoundArr(soundArr), item => item.name == sound);

            sfx.source.Play();
        }

        public void PlayBackground(string sound, string soundArr, bool shouldFade = true)
        {
            if (backgroundMusic1IsPlaying)
            {
                backgroundMusic1 = Array.Find(GetSoundArr(soundArr), item => item.name == sound);
                backgroundMusic1.source.Play();
            }
            else
            {
                backgroundMusic2 = Array.Find(GetSoundArr(soundArr), item => item.name == sound);
                backgroundMusic2.source.Play();
            }
        }

        public void PauseBackground()
        {
            if (backgroundMusic1IsPlaying)
                backgroundMusic1.source.Pause();
            else
                backgroundMusic2.source.Pause();
        }

        public void UnPauseBackground()
        {
            if (backgroundMusic1IsPlaying)
                backgroundMusic1.source.UnPause();
            else
                backgroundMusic2.source.UnPause();
        }

        public void StopBackground()
        {
            if (backgroundMusic1IsPlaying)
                backgroundMusic1.source.Stop();
            else
                backgroundMusic2.source.Stop();
        }

        public void ChangeAudio(string sound)
        {

        }


        // ************************ PRIVATE METHODS ************************ //

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            //Initialize all the sounds with their own AudioSource
            InitializeSoundArray(animation);
            InitializeSoundArray(arena_ambience);
            InitializeSoundArray(arena_battle_music);
            InitializeSoundArray(arena_events);
            InitializeSoundArray(items_common);
            InitializeSoundArray(items_sword);
            InitializeSoundArray(minotaur);
            InitializeSoundArray(player_dash);
            InitializeSoundArray(player_death);
            InitializeSoundArray(player_hit);
            InitializeSoundArray(player_run);
            InitializeSoundArray(player_walk);
            InitializeSoundArray(zeus);

            backgroundMusic1IsPlaying = true;
        }

        void InitializeSoundArray(Sound[] soundArray)
        {
            foreach (Sound s in soundArray)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.outputAudioMixerGroup = mixerGroup;
            }
        }

        Sound[] GetSoundArr(string soundArr)
        {
            switch (soundArr)
            {
                case ("animation"): return animation;
                case ("arena_ambience"): return arena_ambience;
                case ("arena_battle_music"): return arena_battle_music;
                case ("arena_events"): return arena_events;
                case ("items_common"): return items_common;
                case ("items_sword"): return items_sword;
                case ("minotaur"): return minotaur;
                case ("player_dash"): return player_dash;
                case ("player_death"): return player_death;
                case ("player_hit"): return player_hit;
                case ("player_run"): return player_run;
                case ("player_walk"): return player_walk;
                case ("zeus"): return zeus;
                default: return null;
            }
        }

        private IEnumerator FadeIn(float timeToFade, bool shouldFade = true)
        {
            Debug.Log("Start coroutine FadeIn");

            if (!shouldFade)
            {
                sfx.source.volume = 1.0f;
                yield break;
            }

            float startVolume = sfx.source.volume;
            sfx.source.volume = 0.0f;

            while (sfx.source.volume < 1)
            {
                sfx.source.volume += startVolume * Time.deltaTime / timeToFade;

                yield return null;
            }

            sfx.source.volume = startVolume;
        }

        private IEnumerator WaitBeforeFadeIn(float timeToFade, bool shouldFade)
        {
            Debug.Log("Start coroutine WaitBeforeFadeIn");

            yield return new WaitForSeconds(timeToFade * threshold);

            sfx.source.Play();
            StartCoroutine(FadeIn(timeToFade, shouldFade));
        }

        private IEnumerator FadeOut(float timeToFade, bool shouldFade = true)
        {
            Debug.Log("Start coroutine FadeOut");

            if (!shouldFade)
            {
                sfx.source.Stop();
                yield break;
            }

            float startVolume = sfx.source.volume;

            while (sfx.source.volume > 0)
            {
                sfx.source.volume -= startVolume * Time.deltaTime / timeToFade;

                yield return null;
            }

            sfx.source.Stop();
            sfx.source.volume = startVolume;
        }


        // ************************ TESTING PURPOSES ************************ //

        void Start()
        {
            Test();
        }

        public void Test()
        {
            Debug.Log("In function test()");
            PlayBackground("animation_arena_entering", "animation");
            StartCoroutine(Test1());
        }

        public IEnumerator Test1()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test1");
            PauseBackground();
            StartCoroutine(Test2());
        }

        public IEnumerator Test2()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test2");
            UnPauseBackground();
            StartCoroutine(Test3());

        }

        public IEnumerator Test3()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test3");
            StopBackground();
        }
    }
}