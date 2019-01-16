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
        if (!firstPick)
        {
            timeline.Play();
            firstPick = true;
        }
    }
}
