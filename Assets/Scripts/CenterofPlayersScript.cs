using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterofPlayersScript : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;
    //public GameObject camera;
    public float zoomscale = 1;

    Vector3 p1position;
    Vector3 p2position;
    Vector3 offsetFromPlayerCenter;

	// Use this for initialization
	void Start ()
    {
        offsetFromPlayerCenter = transform.position - (player2.transform.position + player1.transform.position) / 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        p1position = player1.transform.position;
        p2position = player2.transform.position;

        float playerDistanceX = System.Math.Abs(p1position.x - p2position.x);
        float playerDistanceZ = System.Math.Abs(p1position.z - p2position.z);

        float maxPlayerDistance = playerDistanceX > playerDistanceZ ? playerDistanceX : playerDistanceZ;

        transform.position = new Vector3((p2position.x + p1position.x) / 2, offsetFromPlayerCenter.y, (p2position.z +p1position.z) / 2 + offsetFromPlayerCenter.z);

        transform.position += Vector3.Normalize(transform.position) * maxPlayerDistance * zoomscale;
    }
}
