using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodsGame
{
    public class CuttingSkill : CooldownSkill
    {
        public PlayerBehaviour player;

        public CuttingSkill(PlayerBehaviour player) : base(2,1,1)
        {
            this.player = player;
        }

        public override void Execute()
        {
            base.Execute();
            Debug.Log("Rope Interaction, start animation");
        }

    }
}

