using UnityEngine;

namespace GodsGame
{
    public class SwordSkill : CooldownSkill<PlayerBehaviour>
    {
        public override void StartExecute(bool startCooldown)
        {
            base.StartExecute(startCooldown);
            Debug.Log("Sword Slash Effect : WOW");
        }
    }
}