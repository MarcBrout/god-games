using GodsGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEvent : MonoBehaviour {

    public Canvas canvas;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void OnActivate(Activable activable, Trigger trigger)
    {
        animator.SetTrigger("FadeIn");
    }

    public void OnDeactivate(Activable activable, Trigger trigger)
    {
        animator.SetTrigger("FadeOut");
    }
}
