using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class FenceEvents : MonoBehaviour
    {
        public Animator _animator;
        
        private string _upParam = "GoUp";
        private string _downParam = "GoDown";

        // Use this for initialization
        void Start()
        {

        }

        public void OnActivate(Activable activable, Trigger trigger)
        {
            _animator.SetTrigger(_downParam);
        }

        public void OnDeactivate(Activable activable, Trigger trigger)
        {
            _animator.SetTrigger(_upParam);
        }
    }
}