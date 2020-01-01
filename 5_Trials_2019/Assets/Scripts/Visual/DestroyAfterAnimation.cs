using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitForAnimation());
    }

    public IEnumerator WaitForAnimation()
    {
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
}
