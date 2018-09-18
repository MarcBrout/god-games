using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManagerScript : MonoBehaviour {

    public List<PressureplateScript> pressureplates;
    private List<bool> pressureplateState = new List<bool>();

    // Use this for initialization
    void Start () {

        for (int i = 0; i < pressureplates.Count; i++)
        {
            pressureplates[i].Init(this, i+1);
            pressureplateState.Add(true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void disableTrap(int id) {
        
        pressureplateState[id-1] = false;
        
        if (areAllTrapsDisabled()) {
            enableAllTrapsExpectOne(id);
        }
    }

    bool areAllTrapsDisabled() {

        foreach(bool state in pressureplateState)
        {
            if (state) {
                return false;
            }
        }

        return true;
    }

    void enableAllTrapsExpectOne(int id) {
        for (int i = 0; i < pressureplateState.Count; i++)
        {
            if (i != id-1)
            {
                pressureplateState[i] = true;
                pressureplates[i].enableTrap();
            }
        }
    }
}
