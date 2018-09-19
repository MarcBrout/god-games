using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	
	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

}
