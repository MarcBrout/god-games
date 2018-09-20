using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurplateTriggerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<PressureplateScript>().plateTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponentInParent<PressureplateScript>().plateTriggerExit(other);

    }
}
