using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{

    public class SwordAnimationEvents : MonoBehaviour
    {
        public Damager _damager;

        public void ActivateSword()
        {
            _damager.EnableDamage();
        }

        public void DeactivateSword()
        {
            _damager.DisableDamage();
        }

    }
}
