using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour {

    public Vector3 pickUpPosition;
    public Vector3 pickUpRotations;
    public bool isThrowable = true;
    public CooldownSkill skill;


    public float throwForce;
    public float cooldownDuration;

    private float _nextReadyTime;

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
