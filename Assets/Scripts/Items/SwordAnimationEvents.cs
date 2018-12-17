using UnityEngine;

namespace GodsGame
{
    public class SwordAnimationEvents : MonoBehaviour
    {
        public Damager damager;
        public AudioSource _audio;

        public void ActivateSword()
        {
            damager.EnableDamage();
        }

        public void DeactivateSword()
        {
            damager.DisableDamage();
        }

        public void SwordAttackSound()
        {
            AudioManager.Instance.PlayRandomSfx3D("items_sword_hit_nothing", ref _audio);
        }
    }
}
