using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class ColumnEvents : MonoBehaviour
    {
        public Animator _animator;
        public GameObject[] _ropes;

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
    }
}