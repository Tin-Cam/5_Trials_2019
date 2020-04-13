﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    private PlayerMove playerMove;

    public bool canAttack;
    public float holdTime;

    public Animator animator;

    void Start()
    {
        audioManager = AudioManager.instance;
        playerMove = GetComponent<PlayerMove>();
    }


    void Update()
    {
        if (!canAttack)
            return;

        Attack();
    }

    //Stops the player from moving, then starts the Attack Coroutine
    private void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            audioManager.Play("Player_Attack");
            canAttack = false;
            playerMove.canMove = false;
            StartCoroutine("AttackCo");
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetTrigger("Attack");
        yield return WaitForAnimation("Attack");
        DefaultState();
    }

    public void DefaultState()
    {
        animator.SetTrigger("Idle");
        canAttack = true;
        playerMove.canMove = true;
    }

    //Finishes when an animation stops playing
    public IEnumerator WaitForAnimation(string animation)
    {
        //Wait for animation to start playing
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Waiting for " + animation);

        //Waits for animation to finish
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            yield return new WaitForFixedUpdate();
        }
    }
}

