using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterofPlayersScript : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;
    public GameObject camera;

    Vector3 p1position;
    Vector3 p2position;
    Vector3 cameraposition;

	// Use this for initialization
	void Start ()
    {
        cameraposition = camera.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        p1position = player1.transform.position;
        p2position = player2.transform.position;

        float playerDistanceX = System.Math.Abs(p1position.x - p2position.x);
        float playerDistanceZ = System.Math.Abs(p1position.z - p2position.z);

        float maxPlayerDistance = playerDistanceX > playerDistanceZ ? playerDistanceX : playerDistanceZ;

        transform.position = new Vector3(p1position.x +  (p2position.x - p1position.x) / 2, 0, p1position.z + (p2position.z - p1position.z) / 2);

        transform.position += Vector3.Normalize(cameraposition) * maxPlayerDistance;
    }
}
