using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour {

    public Vector3 pickUpPosition;
    public Vector3 pickUpRotations;
    public bool isThrowable = true;

    public float throwForce;
    public float cooldownDuration;

    private float _nextReadyTime;
    private Rigidbody _rb;

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void PickUpItem(GameObject itemSocket) {
        _rb.isKinematic = true;
        _rb.detectCollisions = false;

        transform.SetParent(itemSocket.transform);
        transform.localPosition = pickUpPosition;
        transform.localEulerAngles = pickUpRotations;
    }

    public void DropItem() {
        _rb.isKinematic = false;
        _rb.detectCollisions = true;
        transform.SetParent(null);
    }

    public void ThrowItem(Transform direction) {
        DropItem();
        _rb.AddForce(direction.forward * throwForce);
    }

    public void UseItem() {

        if (Time.time > _nextReadyTime)
        {
            ExecuteItem();
            _nextReadyTime = Time.time + cooldownDuration;
        }
    }

    public abstract void ExecuteItem();

    public float GetCooldown() {

        if (_nextReadyTime == 0)
            return 0;

        return _nextReadyTime - Time.time;
    }

    public bool ItemReady(){
        return GetCooldown() <= 0;
    }
}
