using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEnabler : MonoBehaviour {

    public GameObject[] _objects;

	void Update () {
        if (cInput.GetKeyDown("UseItem_P1"))
        {
            foreach (GameObject _obj in _objects)
            {
                _obj.SetActive(!_obj.active);
            }
        }
	}
}
