using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    private Animator animator;
    private bool menuOn;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void BeginMenu()
    {
        if (!menuOn)
        {
            animator.SetTrigger("FadeIn");
            menuOn = true;
        }
        else
        {
            animator.SetTrigger("FadeOut");
            menuOn = false;
        }
    }
}
