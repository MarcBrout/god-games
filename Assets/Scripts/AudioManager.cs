/*
 * Audio class used to change between songs.
 * By: Demis Terborg
 * 
 * HOWTO: Add AudioClips
 * 1. Name the AudioClip by adding an enum under 'public enum AudioTrack {}' (line 25).
 * 2. Declare an AudioClip using the same naming scheme (line 45).
 * 3. Add the AudioClip to the switch case in function 'AudioClip GetAudioClip(AudioTrack track) {}' (line 172).
 * 4. Attach an audio file to the AudioClip by dragging and dropping the file in the inspector view.
 * 
 * HOWTO: Access public methods
 * 1. Make a public reference to AudioManager in your script.
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

        // The volume threshold when the next audioSource should fade in. Should be between 0.0f and 1.0f.
        public float threshold;

        // The AudioSource components attached to the AudioManager object.
        public AudioSource audioSource1, audioSource2, audioSource3;

        // A variable to keep track of which AudioSource is currently playing.
        private bool audioSource1Playing;

        // The AudioClip objects. 
        public AudioClip[] Music;
        public AudioClip[] Dash;

        // Singleton instance.
        public static AudioManager instance = null;


        // ************************ PUBLIC METHODS ************************ //

        // Plays AudioClip depending on which clip has been selected.
        // Also checks which AudioSource should be used, to prevent two AudioSources from playing simultaneously.
        public void playAudio(AudioTrack track, bool shouldFade = true)
        {
            if (audioSource1Playing)
            {
                audioSource1.clip = GetAudioClip(track);
                audioSource1.Play();
                StartCoroutine(FadeIn(timeToFade, shouldFade));
            }
            else
            {
                audioSource2.clip = GetAudioClip(track);
                audioSource2.Play();
                StartCoroutine(FadeIn(timeToFade, shouldFade));
            }
        }

        public void PlayRandomAudio()
        {
            audioSource3.clip = Dash[Random.Range(0, Dash.Length)];
            audioSource3.Play();
        }

        public void stopAudio(bool shouldFade = true)
        {
                StartCoroutine(FadeOut(timeToFade, shouldFade));
        }

        public void pauseAudio()
        {
            if (audioSource1Playing)
                audioSource1.Pause();
            else
                audioSource2.Pause();
        }

        public void unPauseAudio()
        {
            if (audioSource1Playing)
                audioSource1.UnPause();
            else
                audioSource2.UnPause();
        }

        // Checks which AudioSource is currently playing. Fades-out the currently played audio,
        // While fading-in the chosen AudioClip.
        public void changeAudio(AudioTrack track, bool shouldFade = true)
        {
            if (audioSource1Playing)
            {
                StartCoroutine(FadeOut(timeToFade, shouldFade));
                audioSource2.clip = GetAudioClip(track);
                StartCoroutine(WaitBeforeFadeIn(timeToFade, shouldFade));

                audioSource1Playing = false;
            }
            else
            {
                StartCoroutine(FadeOut(timeToFade, shouldFade));
                audioSource1.clip = GetAudioClip(track);
                StartCoroutine(WaitBeforeFadeIn(timeToFade, shouldFade));

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

        // Returns the AudioClip that corresponds to the enum value.
        private AudioClip GetAudioClip(AudioTrack track)
        {
            switch (track)
            {
                case AudioTrack.ThemeSong: return Music[0];
                case AudioTrack.Fight: return Music[1];
                case AudioTrack.PauseMenu: return Music[2];
                default: return null;
            }
        }

        // Coroutine to fade-in the AudioSource by lowering the volume.
        private IEnumerator FadeIn(float timeToFade, bool shouldFade = true)
        {
            Debug.Log("Start coroutine FadeIn");

            if (audioSource1Playing)
            {
                // No fade added, early return
                if (!shouldFade)
                {
                    audioSource1.volume = 1.0f;
                    yield break;
                }

                float startVolume = audioSource1.volume;
                audioSource1.volume = 0.0f;

                while (audioSource1.volume < 1)
                {
                    audioSource1.volume += startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }

                audioSource1.volume = startVolume;
            }
            else
            {
                // No fade added, early return
                if (!shouldFade)
                {
                    audioSource2.volume = 1.0f;
                    yield break;
                }

                float startVolume = audioSource2.volume;
                audioSource2.volume = 0.0f;

                while (audioSource2.volume < 1)
                {
                    audioSource2.volume += startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }

                audioSource2.volume = startVolume;
            }
        }

        // Wait for volume to reach certain threshold before fading in 
        private IEnumerator WaitBeforeFadeIn(float timeToFade, bool shouldFade)
        {
            yield return new WaitForSeconds(timeToFade * threshold);

            if (audioSource1Playing)
            {
                audioSource1.Play();
                StartCoroutine(FadeIn(timeToFade, shouldFade));
            }
            else
            {
                audioSource2.Play();
                StartCoroutine(FadeIn(timeToFade, shouldFade));
            }
        }

        // Coroutine to fade-out the AudioSource by lowering the volume.
        private IEnumerator FadeOut(float timeToFade, bool shouldFade = true)
        {
            Debug.Log("Start coroutine FadeOut");

            if (audioSource1Playing)
            {
                // No fade added, early return
                if (!shouldFade)
                {
                    audioSource1.Stop();
                    yield break;
                }

                float startVolume = audioSource1.volume;

                while (audioSource1.volume > 0)
                {
                    audioSource1.volume -= startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }

                audioSource1.Stop();
                audioSource1.volume = startVolume;
            }
            else
            {
                // No fade added, early return
                if (!shouldFade)
                {
                    audioSource2.Stop();
                    yield break;
                }

                float startVolume = audioSource2.volume;

                while (audioSource2.volume > 0)
                {
                    audioSource2.volume -= startVolume * Time.deltaTime / timeToFade;

                    yield return null;
                }

                audioSource2.Stop();
                audioSource2.volume = startVolume;
            }
        }


        // ************************ TESTING PURPOSES ************************ //

        private void Start()
        {
            //test();
            PlayRandomAudio();
        }

        public void test()
        {
            Debug.Log("In function test()");
            playAudio(AudioTrack.Fight);
            StartCoroutine(Test1());
        }

        public IEnumerator Test1()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test1");
            changeAudio(AudioTrack.ThemeSong);
            StartCoroutine(Test2());
        }

        public IEnumerator Test2()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test2");
            changeAudio(AudioTrack.Fight);
            StartCoroutine(Test3());

        }

        public IEnumerator Test3()
        {
            yield return new WaitForSeconds(5);
            Debug.Log("In IEnumerator Test3");
            changeAudio(AudioTrack.PauseMenu);
        }
    }
}