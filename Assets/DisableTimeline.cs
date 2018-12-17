using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class DisableTimeline : MonoBehaviour {

    public PlayableDirector timeline;
    public Canvas canvas;

    private void OnEnable()
    {
        timeline.enabled = false;
        canvas.enabled = false;
    }
}
