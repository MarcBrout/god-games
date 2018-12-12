using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMinotaurBehaviour : MonoBehaviour {

    public Panda.PandaBehaviour behaviour;
    public GodsGames.MinotaurTasks tasks;

    public void OnEnable()
    {
        behaviour.enabled = true;
        tasks.enabled = true;
    }
}
