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

        public Sound[] animation, arena_ambience, arena_battle_music
            , arena_events, items_common, items_sword, minotaur, player_dash
            , player_death, player_hit, player_run, player_walk, zeus;

        private Sound backgroundMusic1, backgroundMusic2, sfx;

        public float timeToFade, threshold;


        // ************************ PUBLIC METHODS ************************ //

        public void Play(string sound, string soundArr)
        {
            sfx = Array.Find(getSoundArr(soundArr), item => item.name == sound);
            if (sfx == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            sfx.source.Play();
        }

        public void Pause(string sound, string soundArr)
        {
            sfx = Array.Find(getSoundArr(soundArr), item => item.name == sound);
            if (sfx == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            sfx.source.Pause();
        }

        public void UnPause(string sound, string soundArr)
        {
            sfx = Array.Find(getSoundArr(soundArr), item => item.name == sound);
            if (sfx == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            sfx.source.UnPause();
        }

        public void Stop(string sound, string soundArr)
        {
            sfx = Array.Find(getSoundArr(soundArr), item => item.name == sound);
            if (sfx == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            sfx.source.Stop();
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
            InitializeSounds(animation);
            InitializeSounds(arena_ambience);
            InitializeSounds(arena_battle_music);
            InitializeSounds(arena_events);
            InitializeSounds(items_common);
            InitializeSounds(items_sword);
            InitializeSounds(minotaur);
            InitializeSounds(player_dash);
            InitializeSounds(player_death);
            InitializeSounds(player_hit);
            InitializeSounds(player_run);
            InitializeSounds(player_walk);
            InitializeSounds(zeus);
        }

        void InitializeSounds(Sound[] soundArray)
        {
            foreach (Sound s in soundArray)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.outputAudioMixerGroup = mixerGroup;
            }
        }

        Sound[] getSoundArr(string soundArr)
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
                default:
                    Debug.LogWarning("Sound: " + name + " not found!");
                    return null;
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

        void Start()
        {
            Play("cavern_view", "animation");
            //Play("arena3");
        }

        public void Test()
        {
            Debug.Log("In function test()");
            StartCoroutine(Test1());
        }

        public IEnumerator Test1()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test1");
            //Play("arena2");
            StartCoroutine(Test2());
        }

        public IEnumerator Test2()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test2");
            StartCoroutine(Test3());

        }

        public IEnumerator Test3()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test3");
        }
    }
}
