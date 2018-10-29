using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler: MonoBehaviour {

    public GameObject itemUi;
    public GameObject itemSocket;
    public BaseItem Item { get { return _equippedItem; } }
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


            if (itemUi != null)
            {
                itemUi.GetComponent<Image>().sprite = _equippedItem.GetComponent<Image>().sprite;
                itemUi.SetActive(true);
            }
        }
    }

    public bool CanThrow() {
        return _isEquiped && _equippedItem.isThrowable; ;
    }

    public void ThrowItem() {
        _equippedItem.ThrowItem(transform);
        _isEquiped = false;

        if (itemUi != null) {
            itemUi.SetActive(false);
        }
    }

    public bool CanUse() {
        return _isEquiped;
    }

    public bool UseItem() {

        if (_equippedItem.ItemReady()) {
            return _equippedItem.UseItem(); 
        }
        return false;
    }
}
