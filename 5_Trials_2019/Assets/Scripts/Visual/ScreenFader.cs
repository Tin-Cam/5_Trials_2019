using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator FadeIn()
    {
        animator.SetTrigger("FadeIn");
        yield return WaitForAnimation();
    }

    public IEnumerator FadeOut()
    {
        animator.SetTrigger("FadeOut");
        yield return WaitForAnimation();
    }

    //Finishes when an animation stops playing
    public IEnumerator WaitForAnimation()
    {
        yield return new WaitForEndOfFrame();       
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
