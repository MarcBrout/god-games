using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class PressureplateScript : MonoBehaviour
    {

        public GameObject trapScriptGameObject;
        public List<GameObject> coloredTrapParts;
        public Material enabledTrapMaterial;
        public Material disabledTrapmaterial;

        private Animator pressurPlateAnimator;
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
            pressurPlateAnimator = GetComponent<Animator>();

            trapScript = trapScriptGameObject.GetComponent<ITrapInterface>();
            if (trapScript == null)
            {
                throw new Exception("Missing Trapinterface on the Trap Script Gameobject");

            }

            isEnable = true;
            ShowEnabledColor();
            audioSource = GetComponent<AudioSource>();
        }

        public void Init(TrapManagerScript trapManager, int trapmanagerId)
        {
            this.trapManager = trapManager;
            this.trapmanagerId = trapmanagerId;
        }

        public void PlateTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.tag == "Boss")
            {
                if (isEnable)
                {
                    AudioManager.Instance.PlaySfx3D(PLATE_ACTIVATED, PLATE_ARRAY, ref audioSource);
                    pressurPlateAnimator.SetBool("playerEntered", true);
                    trapScript.ActivateTrap();
                    DisableTrap();
                }
                else
                {
                    AudioManager.Instance.PlaySfx3D(PLATE_INACTIVE, PLATE_ARRAY, ref audioSource);
                }
            }
        }

        public void PlateTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.tag == "Boss")
            {
                pressurPlateAnimator.SetBool("playerEntered", false);
                trapScript.DeactivateTrap();

                if (trapManager == null)
                {
                    EnableTrap();
                }
            }
        }

        void DisableTrap()
        {
            isEnable = false;
            pressurPlateAnimator.SetBool("isEnable", false);

            if (trapManager != null)
            {
                trapManager.disableTrap(trapmanagerId);
            }
        }

        public void EnableTrap()
        {
            AudioManager.Instance.PlaySfx3D(PLATE_RESET, PLATE_ARRAY, ref audioSource);

            isEnable = true;
            pressurPlateAnimator.SetBool("isEnable", true);
            trapScript.ShowEnableColor();
        }

        public void ShowDisabledColor() {
            foreach (GameObject obj in coloredTrapParts) {
                obj.GetComponent<Renderer>().material = disabledTrapmaterial;
            }
        }

        public void ShowEnabledColor() {
            foreach (GameObject obj in coloredTrapParts)
            {
                obj.GetComponent<Renderer>().material = enabledTrapMaterial;
            }
        }
    }
}
