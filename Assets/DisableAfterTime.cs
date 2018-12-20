using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour {

    public Canvas canvas;
    public float countdown = 5.0f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void SetEnable()
    {
        animator.SetTrigger("FadeIn");
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Floating"))
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0.0f)
            {
                animator.SetTrigger("FadeOut");
            }
        }
    }
}
