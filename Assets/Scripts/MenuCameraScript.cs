using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraScript : MonoBehaviour {

    // Update is called once per frame
    public float rotationSpeed = 1;
    public float rotationCapLeft;
    public float rotationCapRight;
    public bool enableRotationCap;
    private float _turnPointLeft;
    private float _turnPointRight;

    public void Start()
    {
        if (enableRotationCap) {
            _turnPointLeft = transform.eulerAngles.y - rotationCapLeft;
            _turnPointRight = transform.eulerAngles.y + rotationCapRight;

        }
    }

    void Update () {
        transform.Rotate(Vector3.up * (Time.deltaTime * rotationSpeed));


        if (enableRotationCap) {
            if (rotationSpeed > 0)
            {
                if (transform.eulerAngles.y > _turnPointRight) {
                    rotationSpeed = rotationSpeed * -1;
                }
            }
            else {
                if (transform.eulerAngles.y < _turnPointLeft) {
                    rotationSpeed = rotationSpeed * -1;
                }
            }
        }
    }
}
