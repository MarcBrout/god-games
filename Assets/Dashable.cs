using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodsGame;

public class Dashable : MonoBehaviour {

    public DashSkill skill;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.gameObject.layer);
        Debug.Log(LayerMask.NameToLayer("Dashable"));
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Dashable") && skill != null && skill.IsDashing)
        {
            Debug.Log("DASHHH");
            collision.collider.enabled = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Dashable"))
        {
            collision.collider.enabled = true;
        }
    }
}
