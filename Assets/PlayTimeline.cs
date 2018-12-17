using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class PlayTimeline : MonoBehaviour {

    public PlayableDirector timeline;
    private bool            firstPick = false;

    public void Launch()
    {
        Debug.Log("Sword Pick Up");
        if (!firstPick)
        {
            Debug.Log("Cinematic Begin");
            timeline.Play();
            firstPick = true;
        }
    }
}
