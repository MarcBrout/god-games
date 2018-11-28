using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEvent : MonoBehaviour {

    public Canvas canvas;


	// Use this for initialization
	void Start () {
		
	}

    public void OnActivate(Activable activable, Trigger trigger)
    {
        canvas.GetComponent<Canvas>().enabled = true;
    }

    public void OnDeactivate(Activable activable, Trigger trigger)
    {
        Debug.Log("LEL");
        canvas.GetComponent<Canvas>().enabled = false;
    }
}
