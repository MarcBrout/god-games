using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace GodsGame
{
    public class ColumnEvents : MonoBehaviour
    {
        public Animator _animator;
        public GameObject[] _ropes;
        private AudioSource _audioSource;

        [Serializable]
        public class ColumnEvent : UnityEvent
        { }

        public ColumnEvent OnColumnDown;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // Use this for initialization
        void Update()
        {
            bool allDeactivated = true;

            foreach (GameObject rope in _ropes)
            {
                if (rope.active)
                {
                    allDeactivated = false;
                    break;
                }
            }
            
            if (allDeactivated)
            {
                _animator.SetTrigger("Fall");
            }
        }

        public void OnActivate(Activable activable, Trigger trigger)
        {
            _animator.SetTrigger("Fall");
        }
        
        public void ColumnCrashed()
        {
            AudioManager.Instance.PlaySfx3D("column_down", "items_common", ref _audioSource);
            OnColumnDown.Invoke();
        }
    }
}