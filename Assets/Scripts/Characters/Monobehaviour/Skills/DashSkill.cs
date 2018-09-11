using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class DashSkill : CooldownSkill
    {
        public PlayerBehaviour player;

        public DashSkill(PlayerBehaviour player) : base(3, 3, 3)
        {
            this.player = player;
        }

        public void Init()
        {
            maxChargeNumber = 3;
            currentChargeNumber = 3;
        }

        public override void Execute()
        {
            base.Execute();
            Vector3 dashVelocity = Vector3.Scale(player.CInput, player.dashDistance *
               new Vector3((Mathf.Log(1f / (Time.deltaTime * player.Body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * player.Body.drag + 1)) / -Time.deltaTime)));
            player.Body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

    }
}
