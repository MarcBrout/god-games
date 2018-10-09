using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour {

    public int maxhealth = 3;
    int health = 3;
    float startTime;
    public float lerpSpeed = 100;
    float distance = 40;
    float currentdistance = 40;

    Vector3 startposition;
    Vector3 lerpstart;
    Vector3 lerpend;

    RectTransform rt;
    float length;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        lerpstart = transform.position;
        lerpend = transform.position;
        startposition = transform.position;
        rt = GetComponent<RectTransform>();
        length = rt.rect.width;
        distance = length / maxhealth * transform.lossyScale.x;
    }
	
	// Update is called once per frame
	void Update () {
        float distCovered = (Time.time - startTime) * lerpSpeed;

        float fracJourney = distCovered / currentdistance;

        transform.position = Vector3.Lerp(lerpstart, lerpend, fracJourney);
	}

    public void deprecate()
    {
        health--;

        startTime = Time.time;

        lerpstart = transform.position;
        lerpend = startposition - (3-health) * new Vector3(distance, 0, 0);

        currentdistance = Math.Abs(lerpend.x - lerpstart.x);
    }
}
