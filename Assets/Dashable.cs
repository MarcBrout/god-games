using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodsGame;

public class Dashable : MonoBehaviour {

    public DashSkill skill;
    public Collision previousCollision = null; 
        
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Dashable") && skill != null && skill.IsDashing)
        {
            collision.collider.enabled = false;
        }
        else if (previousCollision != null && previousCollision != collision)
        {
            previousCollision.collider.enabled = true;
        }
        previousCollision = collision;
    }
}
