using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMinotaurBehaviour : MonoBehaviour {

    public Panda.PandaBehaviour behaviour;
    public GodsGames.MinotaurTasks tasks;

    public void OnEnable()
    {
        behaviour.enabled = false;
        tasks.enabled = false;
    }
}
