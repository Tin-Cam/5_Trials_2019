using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    private Image image;
    private Animator animator;

    public float fadeSpeed = 1;
    public float updateInterval;
    public bool fadeOnStart = false;

    private AnimatorScripts animatorScripts;

    void Awake()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
        animatorScripts = GetComponent<AnimatorScripts>();
    }

    private void Start()
    {
        //if (fadeOnStart)
        //    StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        yield return animatorScripts.PlayWholeAnimationRealTime("Fade_In", 0);
    }

    public IEnumerator FadeOut()
    {
        yield return animatorScripts.PlayWholeAnimationRealTime("Fade_Out", 0);
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
