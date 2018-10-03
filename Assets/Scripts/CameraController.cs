﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // the closest point with the lowest rotation is always the position and rotation in the scene

    public GameObject player1;
    public GameObject player2;
    public float zoomscale = 1;
    public float maxrotation = 70;
    public float reachmaxrotationat = 10;


    Vector3 p1position;
    Vector3 p2position;
    Vector3 startrotation;
    float playerDistanceStart;
    Vector3 offsetFromPlayerCenter;

    // Use this for initialization
    void Start()
    {
        p1position = player1.transform.position;
        p2position = player2.transform.position;


        offsetFromPlayerCenter = transform.position - (player2.transform.position + player1.transform.position) / 2;
        startrotation = transform.rotation.eulerAngles;
        playerDistanceStart = Vector3.Distance(p1position, p2position);
    }

    // Update is called once per frame
    void Update()
    {
        p1position = player1.transform.position;
        p2position = player2.transform.position;

        float playerDistanceX = System.Math.Abs(p1position.x - p2position.x);
        float playerDistanceZ = System.Math.Abs(p1position.z - p2position.z);

        float maxPlayerDistance = playerDistanceX > playerDistanceZ ? playerDistanceX : playerDistanceZ;

        transform.position = new Vector3((p2position.x + p1position.x) / 2, offsetFromPlayerCenter.y, (p2position.z + p1position.z) / 2 + offsetFromPlayerCenter.z);

        transform.position += Vector3.Normalize(transform.position) * maxPlayerDistance * zoomscale;

        /*
        float xrotation = startrotation.x + (Vector3.Distance(p1position, p2position) / reachmaxrotationat) * (maxrotation - startrotation.x);

        Debug.Log(Vector3.Distance(p1position, p2position) + "           " + playerDistanceStart + "             " + (maxrotation - startrotation.x));

        xrotation = xrotation > maxrotation ? maxrotation : xrotation;

        transform.rotation = Quaternion.Euler(maxPlayerDistance > reachmaxrotationat ? maxrotation : xrotation ,0 ,0) ;
        */
    }
}
