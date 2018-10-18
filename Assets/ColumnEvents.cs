using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class ColumnEvents : MonoBehaviour
    {
        public Animator _animator;


        // Use this for initialization
        void Start()
        {

        }

        public void OnActivate(Activable activable, Trigger trigger)
        {
            _animator.SetTrigger("Fall");
        }        
    }
}