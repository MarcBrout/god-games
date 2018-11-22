using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class DashSkill : CooldownSkill<PlayerBehaviour>
    {
        public DashSkill(PlayerBehaviour player) : base(player, 3, 3)
        {
        }

        public override void Execute(bool startCooldown = true)
        {
            base.Execute(startCooldown);
            //Vector3 dashVelocity = Vector3.Scale(player.CInput.normalized, player.dashDistance *
            //   new Vector3((Mathf.Log(1f / (Time.deltaTime * player.Body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * player.Body.drag + 1)) / -Time.deltaTime)));
            //player.Body.AddForce(dashVelocity, ForceMode.VelocityChange);
            m_MonoBehaviour.SetMoveVector(m_MonoBehaviour.CInput.normalized * m_MonoBehaviour.dashSpeed);
        }

    }
}
