using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMovement : MonoBehaviour {

    public PlayerBehaviour player;

    private void OnEnable()
    {
        player.moveSpeed = 8;
        player.turnSpeed = 10;
    }
}
