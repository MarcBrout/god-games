/*
 * Audio class used to change between songs
 * 
 * Make a reference to this class, then drag and drop
 * the AudioManager GameObject onto the reference
 * to make use of the public methods.
 * Make sure to put your script in namespace GodsGame.
 * 
 * Demis Terborg
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public enum AudioTrack
    {
        ThemeSong,
        Fight,
        PauseMenu
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public float timeToFade;
        public AudioSource audioSource1, audioSource2;

        private AudioClip audioClip;
        private bool audioSource1Playing;
        public static AudioManager instance = null;

        //Prevent multiple instances from being instantiated
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            audioSource1Playing = true;
        }

        void playAudio(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            StartCoroutine(FadeIn(audioSource, timeToFade));
        }
        public void playAudio(AudioTrack track)
        {
            if (audioSource1Playing)
            {
                playAudio(audioSource1, Resources.Load<AudioClip>("theme"));
            }
            else
            {
                playAudio(audioSource2, Resources.Load<AudioClip>("theme"));
            }
        }

        void stopAudio(AudioSource audioSource)
        {
            StartCoroutine(FadeOut(audioSource, timeToFade));
        }

        void pauseAudio(AudioSource audioSource)
        {
            audioSource.Pause();
        }

        void unPauseAudio(AudioSource audioSource)
        {
            audioSource.UnPause();
        }

        public void changeAudio(AudioTrack audioTrack)
        {
            switch (audioTrack)
            {
                case AudioTrack.ThemeSong:
                    transition(Resources.Load<AudioClip>("theme"));
                    break;
                case AudioTrack.Fight:
                    transition(Resources.Load<AudioClip>("test1"));
                    break;
                case AudioTrack.PauseMenu:
                    break;
            }
        }

        //TODO: Implement coroutine that triggers playAudio when currentAudio.volume < 0.5f
        void transition(AudioClip clip)
        {
            if (audioSource1Playing)
            {
                stopAudio(audioSource1);
                playAudio(audioSource2, clip);
                audioSource1Playing = false;
            }
            else
            {
                stopAudio(audioSource2);
                playAudio(audioSource1, clip);
                audioSource1Playing = true;
            }
        }

        public void changeVolume(float volume)
        {
            audioSource1.volume = volume;
        }

        IEnumerator FadeIn(AudioSource audioSource, float timeToFade)
        {
            Debug.Log("Start coroutine FadeIn");

            float startVolume = audioSource.volume;
            audioSource.volume = 0.0f;

            while (audioSource.volume < 1)
            {
                audioSource.volume += startVolume * Time.deltaTime / timeToFade;

                yield return null;
            }

            audioSource.volume = startVolume;
        }

        IEnumerator FadeOut(AudioSource audioSource, float timeToFade)
        {
            Debug.Log("Start coroutine FadeOut");

            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / timeToFade;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

        //to test the transitions
        public void test()
        {
            StartCoroutine(Test1());
        }

        IEnumerator Test1()
        {
            yield return new WaitForSeconds(6);
            changeAudio(AudioTrack.ThemeSong);
            StartCoroutine(Test2());
        }

        IEnumerator Test2()
        {
            yield return new WaitForSeconds(6);
            changeAudio(AudioTrack.Fight);
            StartCoroutine(Test3());

        }

        IEnumerator Test3()
        {
            yield return new WaitForSeconds(6);
            changeAudio(AudioTrack.ThemeSong);
        }
    }
}