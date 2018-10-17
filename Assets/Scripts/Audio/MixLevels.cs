using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void SetSfxLevel(float sfxLvl)
    {
        masterMixer.SetFloat("sfxVol", sfxLvl);
    }

    public void SetMusicLevel(float musicLvl)
    {
        masterMixer.SetFloat("musicVol", musicLvl);
    }

    public void SetMasterLevel(float masterLvl)
    {
        masterMixer.SetFloat("masterVol", masterLvl);
    }
}
