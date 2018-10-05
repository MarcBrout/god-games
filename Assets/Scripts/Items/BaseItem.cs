using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour {

    public Vector3 pickUpPosition;
    public Vector3 pickUpRotations;
    public bool isThrowable = true;

    public float throwForce;
    public float cooldown;

    public void PickUpItem(GameObject itemSocket) {
        GetComponent<Rigidbody>().isKinematic = true;

        transform.parent = itemSocket.transform;
        transform.localPosition = pickUpPosition;
        transform.localEulerAngles = pickUpRotations;
    }

    public void DropItem() {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = null;
    }

    public void ThrowItem(Transform direction) {
        DropItem();
        GetComponent<Rigidbody>().AddForce(direction.forward * throwForce);
    }
}
