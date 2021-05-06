using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScreenFader : MonoBehaviour
{
    private Image image;
    private Animator animator;

    public float fadeAnimationSpeed = 1;
    public float fadeSpeed = 1;
    public float updateInterval;
    public bool fadeOnStart = false;

    public UnityEvent e_FadeIn;
    public UnityEvent e_FadeOut;

    private AnimatorScripts animatorScripts;

    void Awake()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
        animatorScripts = GetComponent<AnimatorScripts>();
    }

    private void Start()
    {
        if (fadeOnStart)
            StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        animator.speed = fadeAnimationSpeed;
        yield return animatorScripts.PlayWholeAnimationRealTime("Fade_In", 0);
        e_FadeIn.Invoke();
    }

    public IEnumerator FadeOut()
    {
        animator.speed = fadeAnimationSpeed;
        yield return animatorScripts.PlayWholeAnimationRealTime("Fade_Out", 0);
        e_FadeOut.Invoke();
    }

    public void FadeMid(bool state)
    {
        StopAllCoroutines();
        if(state)
            animatorScripts.PlayAnimation("Fade_Mid");
        else
            animatorScripts.PlayAnimation("Fade_Nothing");
    }

    //Finishes when an animation stops playing
    public IEnumerator WaitForAnimation()
    {
        yield return new WaitForEndOfFrame();       
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator PseudoWaitForSeconds(float seconds)
    {
        float startTime = Time.realtimeSinceStartup;
        float currentTime = startTime;

        while (currentTime <= startTime + seconds)
        {
            currentTime = Time.realtimeSinceStartup;
            yield return new WaitForEndOfFrame();
        }
    }
}
