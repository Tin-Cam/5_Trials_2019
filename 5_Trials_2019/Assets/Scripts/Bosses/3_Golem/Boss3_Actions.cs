﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation
{
    //Animation Triggers
    public static readonly string Idle = "Idle";

    public static readonly string AttackStandard = "Atk_Standard";
    public static readonly string AttackDesperation = "Atk_Desperation";
    public static readonly string Fire = "Fire";
    public static readonly string Retaliate = "Retaliate";
}

public class Boss3_Actions : _ActionBase
{
    //public Laser laser;

    private Boss3_Controller controller;
    private LaserManager laserManager;
    private LookAtTarget lookAtTarget;
    private Animator animator;
    private GameObject player;

    public Laser laserRef;
    public RockLaser rockLaser;
    public GameObject spreadShot;
    public GameObject[] desperationTargets;
    public float pushbackIntensity;

    public void Init()
    {
        controller = GetComponent<Boss3_Controller>();
        laserManager = GetComponent<LaserManager>();
        lookAtTarget = GetComponentInChildren<LookAtTarget>();
        animator = GetComponent<Animator>();
        player = controller.player;

        //actionList.Add("Idle");
        actionList.Add("ShootPlayer");
        //actionList.Add("Retaliate");
        actionList.Add("SweepAttack");
        actionList.Add("DesperationAttack");
    }

    //ACTIONS ---------------------

    //Action 0 - Idle
    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(1);
        DefaultState();
    }

    //Action 1 - Shoot Player
    public IEnumerator ShootPlayer()
    {
        lookAtTarget.isAiming = false;
        Quaternion angle = lookAtTarget.aimAngle;

        //Indicate
        animator.SetTrigger(BossAnimation.AttackStandard);
        yield return laserManager.IndicateLaser(1, angle);
        
        //Fire
        animator.SetTrigger(BossAnimation.Fire);
        yield return laserManager.ShootLaser(lookAtTarget.aimAngle);

        DefaultState();
    }

    //Action 2 - Retaliate
    public IEnumerator Retaliate()
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        animator.SetTrigger(BossAnimation.Retaliate);

        yield return new WaitForEndOfFrame();
        yield return WaitForAnimation("Boss3_Retaliate");
        yield return new WaitForSeconds(1);
        SpreadShot();

        yield return new WaitForSeconds(1);
        
        DefaultState();
    }

    //Action 2.1 - Pushback player
    public void Pushback()
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        StartCoroutine(playerMove.knockBack(Vector2.down, pushbackIntensity));
    }

    //Action 2.2 - Retaliate after pushback (using a spreadshot)
    public void SpreadShot()
    {
        animator.SetTrigger(BossAnimation.AttackStandard);
        animator.SetTrigger(BossAnimation.Fire);
        spreadShot.transform.position = new Vector3(0, -1, 0);
        Instantiate(spreadShot, transform);
    }
 
    //Action 3 - Sweeping Attack
    public IEnumerator SweepAttack()
    {
        RockLaser temp = Instantiate(rockLaser, transform);

        //Randomise direction to move Rock Laser
        temp.isMirror = (Random.value > 0.5f);

        DefaultState();
        yield break;
    }

    //Action 4 - Desperation Attack
    public IEnumerator DesperationAttack()
    {
        //Pick a target to aim at
        Transform target = desperationTargets[Random.Range(0, desperationTargets.Length)].transform;

        //Charge Attack
        ShowDesperationFilter(true);
        lookAtTarget.ChangeTarget(target);
        animator.SetTrigger(BossAnimation.AttackDesperation);
        yield return new WaitForSeconds(2);

        //Shoot
        Quaternion targetAngle = CalculateAim(target.position);
        animator.SetTrigger(BossAnimation.Fire);
        yield return laserManager.ShootLaser(targetAngle, BigLaser());

        ShowDesperationFilter(false);
        DefaultState();
    }



    public Quaternion CalculateAim(Vector3 target)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        return targetAngle;
    }

    public void ShootLaser(Vector3 target, Laser laser)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        //laserManager.CreateLaser(targetAngle, laser);
    }

    public Laser BigLaser()
    {
        Laser laser = laserRef;

        laser.gainSpeed = (float)0.1;
        laser.diminishSpeed = (float)0.1;
        laser.holdTime = 5;
        laser.maxWidth = 15;

        return laserRef;
    }

    //Finishes when an animation stops playing
    public IEnumerator WaitForAnimation(string animation)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Waiting for " + animation);
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            yield return new WaitForFixedUpdate();
        }
    }

    public override void DefaultState()
    {
        animator.SetTrigger(BossAnimation.Idle);
        lookAtTarget.isAiming = true;
        lookAtTarget.ChangeTarget(player.transform);
        StartCoroutine(controller.NextAction());
    }
}
