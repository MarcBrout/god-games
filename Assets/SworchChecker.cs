using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SworchChecker : MonoBehaviour {
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14 && other.transform.root.gameObject.layer != 9)
        {
            other.transform.SetPositionAndRotation(new Vector3(8.6f, 2f, 5f), new Quaternion());
            
        }
    }
}
