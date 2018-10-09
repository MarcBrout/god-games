using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class SpikeTrapScript : MonoBehaviour, ITrapInterface
    {
        public Material enabledTrapMaterial;
        public Material disabledTrapMaterial;
        public GameObject ringParent;
        private List<GameObject> trapRings;
        private Animator _spikesAnimator;

        void Start()
        {
            _spikesAnimator = GetComponent<Animator>();
            trapRings = new List<GameObject>();

            foreach (Transform child in ringParent.transform) {
                trapRings.Add(child.gameObject);
            }
            ShowEnableColor();
        }

        public void ActivateTrap()
        {
            _spikesAnimator.SetTrigger("Activate");
        }

        public void DeactivateTrap()
        {
            //_spikesAnimator.SetBool("isActivated", false);
        }

        public void ShowDisabledColor()
        {
            foreach (GameObject item in trapRings) {
                item.GetComponent<Renderer>().material = disabledTrapMaterial;
            }
        }

        public void ShowEnableColor()
        {
            foreach (GameObject item in trapRings)
            {
                item.GetComponent<Renderer>().material = enabledTrapMaterial;
            }
        }

    }
}