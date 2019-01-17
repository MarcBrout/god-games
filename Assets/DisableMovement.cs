using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMovement : MonoBehaviour {

    public PlayerBehaviour player;

    private void OnEnable()
    {
        player.moveSpeed = 0;
        player.turnSpeed = 0;
    }
}
