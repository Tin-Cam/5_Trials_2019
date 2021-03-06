﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Explode : MonoBehaviour
{
    public float aimTime;
    public float damageHoldTime;

    private BoxCollider2D col;
    private AnimatorScripts animator;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<AnimatorScripts>();

        col.enabled = false;
        animator.PlayAnimation("Target_Aim", 0);

        StartCoroutine(Target());
    }

    private IEnumerator Target()
    {
        AudioManager.instance.Play("Button_Press", 0.75f, 0.5f);
        yield return new WaitForSeconds(aimTime);

        AudioManager.instance.Play("Thump", 0.75f, 1.0f);
        StartCoroutine(DamageArea());
        yield return animator.PlayWholeAnimation("Target_Explode", 0);

        while (col.enabled == true)
            yield return new WaitForFixedUpdate();
        Destroy(this.gameObject);
    }

    private IEnumerator DamageArea()
    {
        col.enabled = enabled;
        yield return new WaitForSeconds(damageHoldTime);
        col.enabled = false;
    }
}
