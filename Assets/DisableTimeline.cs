using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using GodsGame;

public class DisableTimeline : MonoBehaviour {

    public PlayableDirector     timeline;
    public Canvas               canvas;
    public Panda.BehaviourTree  BehaviourTree;
    public Damageable           damageable;

    private void OnEnable()
    {
        if (timeline)
            timeline.enabled = false;
        if (canvas)
            canvas.enabled = false;
        if (BehaviourTree)
            BehaviourTree.enabled = true;
        if (damageable)
            damageable.DisableInvulnerability();
    }
}
