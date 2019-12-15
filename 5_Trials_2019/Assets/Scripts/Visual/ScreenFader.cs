using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    private Image image;
    private Animator animator;

    public float fadeSpeed = 1;
    public bool fadeOnStart = false;

    void Awake()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (fadeOnStart)
            StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        image.color = new Color(0, 0, 0, 1);

        float alpha = image.color.a;

        while (image.color.a > 0)
        {
            alpha -= fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FadeOut()
    {
        image.color = new Color(0, 0, 0, 0);

        float alpha = image.color.a;

        while (image.color.a < 1)
        {
            alpha += fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            yield return new WaitForEndOfFrame();
        }
    }

    public void FadeMid(bool state)
    {
        StopAllCoroutines();
        float alpha = 0;
        if (state)
            alpha = (float)0.5;

        image.color = new Color(0, 0, 0, alpha);
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
