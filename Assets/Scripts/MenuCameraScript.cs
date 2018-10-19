using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraScript : MonoBehaviour {

    // Update is called once per frame
    public float rotationSpeed = 1;

	void Update () {
        transform.Rotate(Vector3.up * (Time.deltaTime * rotationSpeed));
    }
}
