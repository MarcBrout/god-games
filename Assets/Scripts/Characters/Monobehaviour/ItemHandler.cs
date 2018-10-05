using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler: MonoBehaviour {


    public GameObject itemSocket;
    private bool _isEquiped = false;
    private BaseItem _equippedItem;

    public ItemHandler(GameObject itemSocket) {
        this.itemSocket = itemSocket;

    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Item" && !_isEquiped) {

            _equippedItem = col.transform.GetComponent<BaseItem>();
            _equippedItem.PickUpItem(itemSocket);

            _isEquiped = true;

        }
    }

    public bool CanThrow() {
        return _isEquiped && _equippedItem.isThrowable; ;
    }

    public void ThrowItem() {
        _equippedItem.ThrowItem(transform);
        _isEquiped = false;
    }
}
