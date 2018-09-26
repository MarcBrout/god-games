using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class TrapManagerScript : MonoBehaviour
    {

        public List<PressureplateScript> pressureplates;
        private List<bool> pressureplateState = new List<bool>();
        public float trapEnableDelayInSec;

        // Use this for initialization
        void Start()
        {

            for (int i = 0; i < pressureplates.Count; i++)
            {
                pressureplates[i].Init(this, i + 1);
                pressureplateState.Add(true);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void disableTrap(int id)
        {

            pressureplateState[id - 1] = false;

            if (AreAllTrapsDisabled())
            {
                StartCoroutine(EnableAllTraps());
            }
        }

        bool AreAllTrapsDisabled()
        {

            foreach (bool state in pressureplateState)
            {
                if (state)
                {
                    return false;
                }
            }

            return true;
        }

        IEnumerator EnableAllTraps()
        {

            yield return new WaitForSeconds(trapEnableDelayInSec);

            for (int i = 0; i < pressureplateState.Count; i++)
            {
                pressureplateState[i] = true;
                pressureplates[i].enableTrap();
            }
        }
    }
}
