/* AudioManager class
 * By: Demis Terborg
 * 
 * Handles all the sounds in the game
 * 
 * Access with: FindObjectOfType<AudioManager>().Play();
 *
 */

using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;


namespace GodsGame
{
    public class AudioManager : MonoBehaviour
    {

        public static AudioManager Instance;

        public AudioMixer masterMixer;
        public AudioMixerGroup masterGroup, musicGroup, sfxGroup;

        public float timeToFade, threshold;

        public Sound[]
            animation,
            arena_ambience,
            arena_battle_music,
            arena_events,
            cyclop_throw,
            items_common,
            items_pressure_plates,
            items_sword_hit_metal,
            items_sword_hit_nothing,
            items_sword_hit_wood,
            menu,
            minotaur_arrival,
            minotaur_attack,
            minotaur_before_attack,
            minotaur_charge,
            minotaur_effort,
            minotaur_enrage,
            minotaur_exhausted,
            minotaur_falling,
            minotaur_pain,
            minotaur_step,
            minotaur_walk,
            player_dash,
            player_death, 
            player_hit, 
            player_run, 
            player_walk, 
            zeus,
            zeus_electric_shock,
            zeus_laugh,
            zeus_thunder;

        private Sound backgroundMusic1, backgroundMusic2, sfx;

        private bool backgroundMusic1IsPlaying;


        // ************************ PUBLIC METHODS ************************ //

        public void PlaySfx(string sound, string soundArr)
        {
            sfx = Array.Find(GetSoundArr(soundArr), item => item.name == sound);
            sfx.source.Play();
        }

        // Use when audiosource is placed on object itself (for 3D audio)
        public void PlaySfx3D(string sound, string soundArr, ref AudioSource audioSource)
        {
            Sound s = Array.Find(GetSoundArr(soundArr), item => item.name == sound);
            audioSource.clip = s.clip;
            audioSource.outputAudioMixerGroup = s.mixerGroup;
            audioSource.Play();
        }

        public void PlayRandomSfx(string soundArr)
        {
            sfx = GetSoundArr(soundArr)[UnityEngine.Random.Range(0, GetSoundArr(soundArr).Length)];
            sfx.source.outputAudioMixerGroup = sfxGroup;
            sfx.source.Play();
        }

        // Use when audiosource is placed on object itself (for 3D audio)
        public void PlayRandomSfx3D(string soundArr, ref AudioSource audioSource)
        {
            audioSource.clip = GetSoundArr(soundArr)[UnityEngine.Random.Range(0, GetSoundArr(soundArr).Length)].clip;
            audioSource.Play();
        }

        public void PlayBackground(string sound, string soundArr)
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

        public void ChangeBackGround(string sound, string soundArr)
        {
            if (backgroundMusic1IsPlaying)
            {
                StartCoroutine(FadeOut());
                backgroundMusic2 = Array.Find(GetSoundArr(soundArr), item => item.name == sound);
                StartCoroutine(WaitBeforeFadeIn());

                backgroundMusic1IsPlaying = false;
            }
            else
            {
                StartCoroutine(FadeOut());
                backgroundMusic1 = Array.Find(GetSoundArr(soundArr), item => item.name == sound);
                StartCoroutine(WaitBeforeFadeIn());

                backgroundMusic1IsPlaying = true;
            }
        }


        // ************************ PRIVATE METHODS ************************ //

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            //Initialize all the sound arrays with their own AudioSource
            InitializeSoundArray(animation, MixerGroup.Music);
            InitializeSoundArray(arena_ambience, MixerGroup.Music);
            InitializeSoundArray(arena_battle_music, MixerGroup.Music);
            InitializeSoundArray(arena_events, MixerGroup.Music);
            InitializeSoundArray(cyclop_throw, MixerGroup.Effects);
            InitializeSoundArray(items_common, MixerGroup.Effects);
            InitializeSoundArray(items_pressure_plates, MixerGroup.Effects);
            InitializeSoundArray(items_sword_hit_metal, MixerGroup.Effects);
            InitializeSoundArray(items_sword_hit_nothing, MixerGroup.Effects);
            InitializeSoundArray(items_sword_hit_wood, MixerGroup.Effects);
            InitializeSoundArray(menu, MixerGroup.Effects);
            InitializeSoundArray(minotaur_arrival, MixerGroup.Effects);
            InitializeSoundArray(minotaur_attack, MixerGroup.Effects);
            InitializeSoundArray(minotaur_before_attack, MixerGroup.Effects);
            InitializeSoundArray(minotaur_charge, MixerGroup.Effects);
            InitializeSoundArray(minotaur_effort, MixerGroup.Effects);
            InitializeSoundArray(minotaur_enrage, MixerGroup.Effects);
            InitializeSoundArray(minotaur_exhausted, MixerGroup.Effects);
            InitializeSoundArray(minotaur_falling, MixerGroup.Effects);
            InitializeSoundArray(minotaur_pain, MixerGroup.Effects);
            InitializeSoundArray(minotaur_step, MixerGroup.Effects);
            InitializeSoundArray(minotaur_walk, MixerGroup.Effects);
            InitializeSoundArray(player_dash, MixerGroup.Effects);
            InitializeSoundArray(player_death, MixerGroup.Effects);
            InitializeSoundArray(player_hit, MixerGroup.Effects);
            InitializeSoundArray(player_run, MixerGroup.Effects);
            InitializeSoundArray(player_walk, MixerGroup.Effects);
            InitializeSoundArray(zeus, MixerGroup.Effects);
            InitializeSoundArray(zeus_electric_shock, MixerGroup.Effects);
            InitializeSoundArray(zeus_laugh, MixerGroup.Effects);
            InitializeSoundArray(zeus_thunder, MixerGroup.Effects);

            backgroundMusic1IsPlaying = true;
        }

        private void InitializeSoundArray(Sound[] soundArray, MixerGroup group)
        {
            foreach (Sound s in soundArray)
            {
                switch (group)
                {
                    case MixerGroup.Music:
                        s.source = gameObject.AddComponent<AudioSource>();
                        s.source.clip = s.clip;
                        s.source.outputAudioMixerGroup = musicGroup;
                        break;
                    case MixerGroup.Effects:
                        s.source = gameObject.AddComponent<AudioSource>();
                        s.source.clip = s.clip;
                        s.source.outputAudioMixerGroup = sfxGroup;
                        s.mixerGroup = sfxGroup;
                        break;
                }
            }
        }

        private Sound[] GetSoundArr(string soundArr)
        {
            switch (soundArr)
            {
                case ("animation"): return animation;
                case ("arena_ambience"): return arena_ambience;
                case ("arena_battle_music"): return arena_battle_music;
                case ("arena_events"): return arena_events;
                case ("cyclop_throw"): return cyclop_throw;
                case ("items_common"): return items_common;
                case ("items_pressure_plates"): return items_pressure_plates;
                case ("items_sword_hit_metal"): return items_sword_hit_metal;
                case ("items_sword_hit_nothing"): return items_sword_hit_nothing;
                case ("items_sword_hit_wood"): return items_sword_hit_wood;
                case ("menu"): return menu;
                case ("minotaur_arrival"): return minotaur_arrival;
                case ("minotaur_attack"): return minotaur_attack;
                case ("minotaur_before_attack"): return minotaur_before_attack;
                case ("minotaur_charge"): return minotaur_charge;
                case ("minotaur_effort"): return minotaur_effort;
                case ("minotaur_enrage"): return minotaur_enrage;
                case ("minotaur_exhausted"): return minotaur_exhausted;
                case ("minotaur_falling"): return minotaur_falling;
                case ("minotaur_pain"): return minotaur_pain;
                case ("minotaur_step"): return minotaur_step;
                case ("minotaur_walk"): return minotaur_walk;
                case ("player_dash"): return player_dash;
                case ("player_death"): return player_death;
                case ("player_hit"): return player_hit;
                case ("player_run"): return player_run;
                case ("player_walk"): return player_walk;
                case ("zeus"): return zeus;
                case ("zeus_electric_shock"): return zeus_electric_shock;
                case ("zeus_laugh"): return zeus_laugh;
                case ("zeus_thunder"): return zeus_thunder;
                default: return null;
            }
        }

        private IEnumerator FadeIn(bool shouldFade = true)
        {
            Debug.Log("Start coroutine FadeIn");

            if (backgroundMusic1IsPlaying)
            {
                float startVolume = 1.0f;
                backgroundMusic1.source.volume = 0.0f;
                backgroundMusic1.source.Play();

                while (backgroundMusic1.source.volume < startVolume)
                {
                    backgroundMusic1.source.volume += startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }
            }
            else
            {
                float startVolume = 1.0f;
                backgroundMusic2.source.volume = 0.0f;
                backgroundMusic2.source.Play();

                while (backgroundMusic2.source.volume < startVolume)
                {
                    backgroundMusic2.source.volume += startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }
            }
        }

        private IEnumerator WaitBeforeFadeIn(bool shouldFade = true)
        {
            Debug.Log("Start coroutine WaitBeforeFadeIn");

            yield return new WaitForSeconds(timeToFade * threshold);

            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeOut()
        {
            Debug.Log("Start coroutine FadeOut");

            if (backgroundMusic1IsPlaying)
            {
                float startVolume = backgroundMusic1.source.volume;

                while (backgroundMusic1.source.volume > 0)
                {
                    backgroundMusic1.source.volume -= startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }

                backgroundMusic1.source.Stop();
                backgroundMusic1.source.volume = startVolume;
            }
            else
            {
                float startVolume = backgroundMusic2.source.volume;

                while (backgroundMusic2.source.volume > 0)
                {
                    backgroundMusic2.source.volume -= startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }

                backgroundMusic2.source.Stop();
                backgroundMusic2.source.volume = startVolume;
            }
        }


        // ************************ TESTING PURPOSES ************************ //

        private void Start()
        {
            //Test();
        }

        private void Test()
        {
            Debug.Log("playBackground");
            //PlayBackground("animation_arena_entering", "animation");
            PlaySfx("button_click", "menu");
            StartCoroutine(Test1());
        }

        private IEnumerator Test1()
        {
            yield return new WaitForSeconds(6);
            Debug.Log("ChangeBackGround1");
            //ChangeBackGround("animation_cavern_view", "animation");
            PlaySfx("button_click", "menu");
            //StartCoroutine(Test2());
        }

        private IEnumerator Test2()
        {
            yield return new WaitForSeconds(6);
            Debug.Log("ChangeBackGround2");
            ChangeBackGround("animation_corridor_walking", "animation");
            StartCoroutine(Test3());

        }

        private IEnumerator Test3()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test3");
        }
    }
}