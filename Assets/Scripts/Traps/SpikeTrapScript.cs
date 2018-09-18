using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapScript : MonoBehaviour {

    public Animator spikesAnimator;
    public GameObject border;
    private Color borderColor;
    public Color disabledTrapColor = Color.gray;

    // Use this for initialization
    void Start () {
        borderColor = border.GetComponent<Renderer>().material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ActivateTrap() {
        spikesAnimator.SetBool("isActivated", true);

    }

    public void DeactivateTrap()
    {
        spikesAnimator.SetBool("isActivated", false);

    }

    public void ShowDisabledColor() {
        border.GetComponent<Renderer>().material.SetColor("_Color", disabledTrapColor);
    }

    public void ShowEnableColor() {
        border.GetComponent<Renderer>().material.SetColor("_Color", borderColor);
    }

}
