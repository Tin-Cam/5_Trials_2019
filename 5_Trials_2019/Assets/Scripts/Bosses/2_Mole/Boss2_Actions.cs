﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Actions : _ActionBase
{
    public float idleTime;

    private Boss2_Move move;
    private Boss2_Controller controller;
    private GameObject player;
    private Animator animator;

    public GameObject projectile;
    public SpinningLaser laser;
    public float doubleOffset;
    public float tripleOffset;
    public float moveHoldTime;
    public int rapidMoveAmount;
    public float desperationTime;

    public void Init()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<Boss2_Controller>();
        move = GetComponent<Boss2_Move>();

        player = controller.player;
        isActing = false;

        actionList.Add("Idle");
        //actionList.Add("MoveRandom");
        actionList.Add("Attack");
        actionList.Add("RapidAttack");
        actionList.Add("Desperation");
    }

    //ACTIONS ---------------------------------

    //Default Action - Move randomly across the room
    private IEnumerator MoveRandom()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return move.MovePosition();
            yield return new WaitForSeconds(moveHoldTime);
        }

        Debug.Log("Move Done");
        yield return new WaitForEndOfFrame();
        controller.NextAction();
    }

    //Action 0 - Idle
    private IEnumerator Idle()
    {
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(idleTime);
        StartCoroutine(MoveRandom());
    }

    //Action 1 - Attack
    private IEnumerator Attack()
    {
        //yield return move.MovePosition();
        animator.SetTrigger("Attack");
        Vector3 target = player.transform.position;

        for (int i = 0; i < 5; i++)
        {
            TripleShot(target);
            yield return new WaitForSeconds((float)0.2);
        }

        yield return new WaitForSeconds(1);
        
        StartCoroutine(MoveRandom());
    }

    //Action 2 - Rapid Attack
    private IEnumerator RapidAttack()
    {
        float speed = move.digSpeed;

        move.ChangeSpeed(speed * 2);

        Vector3 target = player.transform.position;

        for (int i = 0; i < rapidMoveAmount; i++)
        {
            yield return move.MovePosition();
            animator.SetTrigger("Attack");
            SingleShot(target);
            yield return new WaitForSeconds((float)0.2);
        }

        move.ChangeSpeed(speed);
        StartCoroutine(MoveRandom());
    }

    //Action 3 - Desperation
    private IEnumerator Desperation()
    {
        yield return move.MovePosition(4);
        animator.SetTrigger("Desperation");

        laser.gameObject.SetActive(true);
        yield return laser.StartSpin();
        yield return new WaitForSeconds(desperationTime);
        yield return laser.EndSpin();

        yield return new WaitForSeconds(1);
        StartCoroutine(MoveRandom());
    }

    //OTHER COMMANDS ----------------------
    void Shoot(float offset, Vector3 target)
    {
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

        //Calculates the direction of the player (and applies an offset to it)
        Vector2 direction = target - gameObject.transform.position;
        Quaternion offsetVector = Quaternion.AngleAxis(offset, Vector3.forward);

        //Applies an offset angle
        direction = offsetVector * direction;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile_Simple>().direction = direction;
    }

    //The following functions shoot different amount of bullets at the same time
    void SingleShot(Vector3 target)
    {
        Shoot(0, target);
    }

    void DoubleShot(Vector3 target)
    {
        Shoot(doubleOffset, target);
        Shoot(-doubleOffset, target);
    }

    void TripleShot(Vector3 target)
    {
        Shoot(0, target);
        Shoot(tripleOffset, target);
        Shoot(-tripleOffset, target);
    }

    public override void DefaultState()
    {
        StopActing();
    }
}
