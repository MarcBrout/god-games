using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace GodsGame
{
    [RequireComponent(typeof(Collider))]
    public class Trigger : MonoBehaviour
    {
        [Serializable]
        public class TriggerEvent : UnityEvent<Activable, Trigger>
        { }

        public bool isEnabled = true;
        public Collider Collider { get { return _collider; } }
        public bool disableAfterEnter = false;
        public float disabledEnterDuration = 3f;
        public bool disableAfterExit = false;
        public float disabledExitDuration = 3f;
        public Activable[] activables;
        public LayerMask triggerableLayers;
        public TriggerEvent TriggerEnterEvent;
        public TriggerEvent TriggerExitEvent;

        private Collider _collider;

        public void Enable()
        {
            isEnabled = true;
        }

        public void Disable()
        {
            isEnabled = false;
        }

        public void Toggle()
        {
            isEnabled = !isEnabled;
        }

        public void DisableFor(float duration)
        {
            isEnabled = false;
            StartCoroutine(EnableAfter(duration));
        }

        // Use this for initialization
        void Start()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerableLayers.Contain(other.gameObject.layer))
            {
                bool triggered = false;

                foreach (Activable activable in activables)
                {
                    if (activable)
                    {
                        TriggerEnterEvent.Invoke(activable, this);
                        if (isEnabled)
                        {
                            activable.EnterActivate(this);
                            triggered = true;
                        }
                    }
                }
                if (triggered && disableAfterEnter) {
                    DisableFor(disabledEnterDuration);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggerableLayers.Contain(other.gameObject.layer))
            {
                bool triggered = false;

                foreach (Activable activable in activables)
                {
                    if (activable)
                    {
                        TriggerExitEvent.Invoke(activable, this);
                        if (isEnabled)
                        {
                            activable.ExitActivate(this);
                        }
                    }
                }
                if (triggered && disableAfterExit)
                {
                    DisableFor(disabledExitDuration);
                }
            }
        }

        public IEnumerator EnableAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isEnabled = true;
        }
    }
}