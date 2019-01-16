using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCyclopAction : MonoBehaviour {

    public Panda.BehaviourTree BehaviourTree;
    public Damageable          damageable;

    private void OnEnable()
    {
        BehaviourTree.enabled = false;
        damageable.EnableInvulnerability(true);    
    }
}
