using UnityEngine.Audio;
using UnityEngine;


namespace GodsGame
{

    public enum MixerGroup
    {
        Master,
        Music,
        Effects
    }


    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [HideInInspector]
        public AudioMixerGroup mixerGroup;

        [HideInInspector]
        public AudioSource source;
    }
}