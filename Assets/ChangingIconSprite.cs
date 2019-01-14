using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingIconSprite : MonoBehaviour {

    public Sprite newSprite;
    public Image Image;

    void OnEnable()
    {
        Image.sprite = newSprite;
    }
}
