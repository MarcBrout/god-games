using UnityEngine;

namespace GodsGame
{
    public class SwordItem : BaseItem
    {
        public override void Start()
        {
            base.Start();
            //skill = new SwordSkill();
            TriggerAnimatorHash = Animator.StringToHash("UseSword");
        }

        public override void UseItem(bool startCooldown = true)
        {
            this.DelayAction(0.6f, () => { m_Rigidbody.detectCollisions = true; });
            base.UseItem(startCooldown);
            this.DelayAction(1f, () => { m_Rigidbody.detectCollisions = false; });
        }
    }
}