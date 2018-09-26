using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class PressureplateScript : MonoBehaviour
    {

        public Animator pressurPlateAnimator;
        public GameObject trapScriptGameObject;
        private ITrapInterface trapScript;
        private bool isEnable;
        private TrapManagerScript trapManager;
        private int trapmanagerId;
        private AudioSource audioSource;

        // Sounds Names
        private string PLATE_ARRAY = "items_pressure_plates";
        private string PLATE_ACTIVATED = "PRESSURE_PLATE_ACTIVATED";
        private string PLATE_RESET = "PRESSURE_PLATE_RESET";
        private string PLATE_INACTIVE = "PRESSURE_PLATE_INACTIVE";

        // Use this for initialization
        void Start()
        {

            trapScript = trapScriptGameObject.GetComponent<ITrapInterface>();
            if (trapScript == null)
            {
                throw new Exception("Missing Trapinterface on the Trap Script Gameobject");

            }

            isEnable = true;
            audioSource = GetComponent<AudioSource>();
        }

        public void Init(TrapManagerScript trapManager, int trapmanagerId)
        {
            this.trapManager = trapManager;
            this.trapmanagerId = trapmanagerId;
        }

        public void plateTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.tag == "Boss")
            {
                if (isEnable)
                {
                    AudioManager.Instance.PlaySfx3D(PLATE_ACTIVATED, PLATE_ARRAY, ref audioSource);
                    pressurPlateAnimator.SetBool("playerEntered", true);
                    trapScript.ActivateTrap();
                    disableTrap();
                }
                else
                {
                    AudioManager.Instance.PlaySfx3D(PLATE_INACTIVE, PLATE_ARRAY, ref audioSource);
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

            if (trapManager != null)
            {
                trapManager.disableTrap(trapmanagerId);
            }
        }

        public void enableTrap()
        {
            AudioManager.Instance.PlaySfx3D(PLATE_RESET, PLATE_ARRAY, ref audioSource);

            isEnable = true;
            pressurPlateAnimator.SetBool("isEnable", true);
            trapScript.ShowEnableColor();
        }
    }
}
