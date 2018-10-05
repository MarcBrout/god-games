using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class PressurplateTriggerScript : MonoBehaviour
    {

        AudioSource clickety;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            GetComponentInParent<PressureplateScript>().PlateTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            GetComponentInParent<PressureplateScript>().PlateTriggerExit(other);

        }
    }
}
