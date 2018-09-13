/*
 * Audio class used to change between songs.
 * By: Demis Terborg
 * 
 * HOWTO: Add AudioClips
 * 1. Name the AudioClip by adding an enum under 'public enum AudioTrack {}'.
 * 2. Declare an AudioClip using the same naming scheme (line 43).
 * 3. Add the AudioClip to the switch case in function 'AudioClip GetAudioClip(AudioTrack track) {}' (line 171).
 * 4. Attach an audio file to the AudioClip by dragging and dropping the file in the inspector view.
 * 
 * HOWTO: Access public methods
 * 1. Make a public reference to this class in your script.
 * 2. Drag and drop the AudioManager GameObject onto the reference in the 'Inspector view'.
 * 3. Access the member functions using the member-access operator ( . ) between the object variable name and the member name.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    // Add AudioTrack to enum here. Use logical names for readability.
    public enum AudioTrack
    {
        ThemeSong,
        Fight,
        PauseMenu
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        // The time it will take to fade in/out a track.
        public float timeToFade;

        // The AudioSource components attached to the AudioManager object.
        public AudioSource audioSource1, audioSource2;

        // A variable to keep track of which AudioSource is currently playing.
        private bool audioSource1Playing;

        // Use same naming scheme as below
        public AudioClip Clip_00, Clip_01, Clip_02;

        // Singleton instance.
        public static AudioManager instance = null;


        // ************************ PUBLIC METHODS ************************ //

        // TODO: add extra param to make FadeOut optional. Get rid of redundant code.
        // Plays AudioClip depending on which clip has been selected.
        // Also checks which AudioSource should be used, to prevent two AudioSources from playing simultaneously.
        public void playAudio(AudioTrack track)
        {
            if (audioSource1Playing)
            {
                audioSource1.clip = GetAudioClip(track);
                audioSource1.Play();
                StartCoroutine(FadeIn(audioSource1, timeToFade));
            }
            else
            {
                audioSource2.clip = GetAudioClip(track);
                audioSource2.Play();
                StartCoroutine(FadeIn(audioSource2, timeToFade));
            }
        }

        // TODO: add extra param to make FadeOut optional. Get rid of redundant code.
        public void stopAudio()
        {
            if (audioSource1Playing)
            {
                StartCoroutine(FadeOut(audioSource1, timeToFade));
            }
            else
            {
                StartCoroutine(FadeOut(audioSource2, timeToFade));
            }
        } 

        // TODO: add extra param to make fadeOut optional. Get rid of redundant code.
        public void pauseAudio()
        {
            if (audioSource1Playing)
            {
                audioSource1.Pause();
            }
            else
            {
                audioSource2.Pause();
            }
        }

        // TODO: add extra param to make fadeIn optional. Get rid of redundant code.
        public void unPauseAudio()
        {
            if (audioSource1Playing)
            {
                audioSource1.UnPause();
            }
            else
            {
                audioSource2.UnPause();
            }
        }

        // TODO: Implement coroutine that triggers playAudio when currentAudio.volume < 0.5f //
        // Checks which AudioSource is currently playing. Fades out the currently played audio,
        // While fading in the chosen AudioClip.
        public void changeAudio(AudioTrack track)
        {
            if (audioSource1Playing)
            {
                StartCoroutine(FadeOut(audioSource1, timeToFade));
                playAudio(ref audioSource2, GetAudioClip(track));

                // Sets audioSource2 as primary AudioSource
                audioSource1Playing = false;
            }
            else
            {
                StartCoroutine(FadeOut(audioSource2, timeToFade));
                playAudio(ref audioSource1, GetAudioClip(track));

                // Sets audioSource1 as primary AudioSource
                audioSource1Playing = true;
            }
        }

        // Change volume depending on Currently playing AudioSource.
        public void changeVolume(float volume)
        {
            if (audioSource1Playing)
                audioSource1.volume = volume;
            else
                audioSource2.volume = volume;
        }


        // ************************ PRIVATE METHODS ************************ //

        // Allows only one instance of AudioManager
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            // Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad(gameObject);

            // Sets audioSource1 as primary AudioSource
            audioSource1Playing = true;
        }

        // TODO: add extra param to make FadeIn optional
        // Used by function ChangeAudio
        private void playAudio(ref AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            StartCoroutine(FadeIn(audioSource, timeToFade));
        }

        // Returns the AudioClip that corresponds to the enum value.
        AudioClip GetAudioClip(AudioTrack track)
        {
            switch (track)
            {
                case AudioTrack.ThemeSong: return Clip_00;
                case AudioTrack.Fight: return Clip_01;
                case AudioTrack.PauseMenu: return Clip_02;
                default: return null;
            }
        }

        // Coroutine to fade-in the AudioSource by lowering the volume.
        private IEnumerator FadeIn(AudioSource audioSource, float timeToFade)
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

        // Coroutine to fade-out the AudioSource by lowering the volume.
        private IEnumerator FadeOut(AudioSource audioSource, float timeToFade)
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


        // ************************ TESTING PURPOSES ************************ //

        public void test()
        {
            playAudio(AudioTrack.Fight);
            StartCoroutine(Test1());
        }

        public IEnumerator Test1()
        {
            yield return new WaitForSeconds(6);
            changeAudio(AudioTrack.ThemeSong);
            StartCoroutine(Test2());
        }

        public IEnumerator Test2()
        {
            yield return new WaitForSeconds(6);
            changeAudio(AudioTrack.Fight);
            StartCoroutine(Test3());

        }

        public IEnumerator Test3()
        {
            yield return new WaitForSeconds(6);
            changeAudio(AudioTrack.PauseMenu);
        }
    }
}