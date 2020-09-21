using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScripts : MonoBehaviour
{
    public Animator animator;

    //Plays Animation
    public void PlayAnimation(string animation)
    {
       PlayAnimation(animation, 0);
    }

    public void PlayAnimation(string animation, int layer)
    {
        animator.Play(animation, layer);
    }

    //Finishes when an animation stops playing
    public IEnumerator PlayWholeAnimation(string animation, int layer)
    {
        animator.Play(animation, layer);
        yield return new WaitForEndOfFrame();
        Debug.Log("Waiting for " + animation);
        while (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
        {
            yield return new WaitForFixedUpdate();
        }
    }

    //Finishes when an animation stops playing
    public IEnumerator PlayWholeAnimationRealTime(string animation, int layer)
    {
        animator.Play(animation, layer);
        yield return new WaitForEndOfFrame();
        Debug.Log("Waiting for " + animation);
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f);
    }

    //NOT USED YET
    private void DefaultState()
    {
        StopCoroutine("PlayWholeAnimation");
    }
}
