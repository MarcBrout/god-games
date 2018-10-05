using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseItem", menuName = "Item")]
public class Item : ScriptableObject {
    public Vector3 pickUpPosition;
    public Vector3 pickUpRotation;

}