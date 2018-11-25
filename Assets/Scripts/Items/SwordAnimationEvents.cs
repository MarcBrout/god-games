using UnityEngine;

namespace GodsGame
{
    public class SwordAnimationEvents : MonoBehaviour
    {
        public Damager damager;

        public void ActivateSword()
        {
            damager.EnableDamage();
        }

        public void DeactivateSword()
        {
            damager.DisableDamage();
        }

    }
}
