using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;
    /*
    Vector3 p1position;
    Vector3 p2position;
    */
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(0, 0, 0);
    }
}
