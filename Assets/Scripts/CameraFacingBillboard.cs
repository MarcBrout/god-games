using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    void Update()
    {
        if (Camera.main.orthographic)
            transform.LookAt(transform.position - Camera.main.transform.forward, Camera.main.transform.up);
        else
            transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
    }
}
