using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace GodsGame
{
    public class Activable : MonoBehaviour
    {
        [Serializable]
        public class ActivateEvent : UnityEvent<Activable, Trigger>
        { }


        [Tooltip("Should the activable ignore any trigger events following the first one")]
        public bool ignoreTriggers = false;
        [Tooltip("Ignore trigger events for X seconds")]
        public float ignoreDuration = 0f;
        public bool isEnabled = true;
        public bool autoCallActivateExit = false;
        public float autoCallDelay = 0f;
        public ActivateEvent OnActivateEnter;
        public ActivateEvent OnActivateEnterDisabled;
        public ActivateEvent OnActivateExit;

        public void EnterActivate(Trigger trigger)
        {
            if (isEnabled)
            {
                OnActivateEnter.Invoke(this, trigger);
                if (autoCallActivateExit)
                {
                    StartCoroutine(CallAfter(autoCallDelay, trigger));
                }
            }
            else
            {
                OnActivateEnterDisabled.Invoke(this, trigger);
            }
        }

        public void ExitActivate(Trigger trigger)
        {
            if (isEnabled && !autoCallActivateExit)
            {
                OnActivateExit.Invoke(this, trigger);
            }
        }

        public IEnumerator CallAfter(float seconds, Trigger trigger)
        {
            yield return new WaitForSeconds(seconds);
            OnActivateExit.Invoke(this, trigger);
        }
    }
}