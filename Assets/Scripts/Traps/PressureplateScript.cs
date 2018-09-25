using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureplateScript : MonoBehaviour {

    public Animator pressurPlateAnimator;
    public GameObject trapScriptGameObject;
    private ITrapInterface trapScript;
    private bool isEnable;
    private TrapManagerScript trapManager;
    private int trapmanagerId;
    private AudioSource[] sounds;

	// Use this for initialization
	void Start () {

        trapScript = trapScriptGameObject.GetComponent<ITrapInterface>();
        if (trapScript == null) {
            throw new Exception("Missing Trapinterface on the Trap Script Gameobject");

        }

        isEnable = true;
        sounds = GetComponents<AudioSource>();
        Debug.Log(sounds.Length);
    }

    public void Init(TrapManagerScript trapManager, int trapmanagerId){
        this.trapManager = trapManager;
        this.trapmanagerId = trapmanagerId;
    }

    public void plateTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" ||  col.gameObject.tag == "Boss")
        {
            if (isEnable) {
                sounds[1].Play();
                pressurPlateAnimator.SetBool("playerEntered", true);
                trapScript.ActivateTrap();
                disableTrap();
            }
                else
            {
                sounds[0].Play();
            }
        }
    }

    public void plateTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Boss")
        {
            pressurPlateAnimator.SetBool("playerEntered", false);
            trapScript.DeactivateTrap();

            if (trapManager == null)
            {
                enableTrap();
            }
        }
    }

    void disableTrap()
    {
        isEnable = false;
        pressurPlateAnimator.SetBool("isEnable", false);

        if (trapManager != null) {
            trapManager.disableTrap(trapmanagerId);
        }
    }

    public void enableTrap()
    {
        sounds[2].Play();
        isEnable = true;
        pressurPlateAnimator.SetBool("isEnable", true);
        trapScript.ShowEnableColor();

    }
}
