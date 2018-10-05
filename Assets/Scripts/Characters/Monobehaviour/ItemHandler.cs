using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler: MonoBehaviour {

    public GameObject itemUi;
    public GameObject itemSocket;
    private bool _isEquiped = false;
    private BaseItem _equippedItem;
    private CooldownSkillUI _cooldownSkillUI;

    public void Start() {
        _cooldownSkillUI = itemUi.GetComponent<CooldownSkillUI>();
    }

    public ItemHandler(GameObject itemSocket) {
        this.itemSocket = itemSocket;

    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Item" && !_isEquiped) {

            _equippedItem = col.transform.GetComponent<BaseItem>();
            _equippedItem.PickUpItem(itemSocket);

            _isEquiped = true;
            itemUi.GetComponent<Image>().sprite = _equippedItem.GetComponent<Image>().sprite;
            itemUi.SetActive(true);
            _cooldownSkillUI.Skill = _equippedItem.GetComponent<BaseItem>().skill;
            _cooldownSkillUI.UpdateUI();

        }
    }

    public bool CanThrow() {
        return _isEquiped && _equippedItem.isThrowable; ;
    }

    public void ThrowItem() {
        _equippedItem.ThrowItem(transform);
        _isEquiped = false;
        itemUi.SetActive(false);
    }

    public void UseItem() {

        if (_equippedItem.ItemReady()) {
            _equippedItem.UseItem();
            _cooldownSkillUI.TransitionToCooldownStart();

        }
    }
}
