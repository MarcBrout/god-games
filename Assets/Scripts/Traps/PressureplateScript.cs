using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureplateScript : MonoBehaviour {

    public Animator pressurPlateAnimator;
    public SpikeTrapScript spikeTrapScript;
    public bool isEnable;
    private TrapManagerScript trapManager;
    public int trapmanagerId;

	// Use this for initialization
	void Start () {
        isEnable = true;
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
                pressurPlateAnimator.SetBool("playerEntered", true);
                spikeTrapScript.ActivateTrap();
                disableTrap();
            }
        }
    }

    public void plateTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Boss")
        {
            pressurPlateAnimator.SetBool("playerEntered", false);
            spikeTrapScript.DeactivateTrap();

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
        isEnable = true;
        pressurPlateAnimator.SetBool("isEnable", true);
        spikeTrapScript.ShowEnableColor();

    }
}
