using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation
{
    //Animation Triggers
    public static readonly string Idle = "Exit";

    public static readonly string AttackStandard = "Atk_Standard";
    public static readonly string AttackDesperation = "Atk_Desperation";
    public static readonly string Fire = "Fire";
}

public class Boss3_Actions : _ActionBase
{
    //public Laser laser;

    private Boss3_Controller controller;
    private LaserManager laserManager;
    private LookAtTarget lookAtTarget;
    private Animator animator;
    private GameObject player;

    public float pushbackIntensity;

    public void Init()
    {
        controller = GetComponent<Boss3_Controller>();
        laserManager = GetComponent<LaserManager>();
        lookAtTarget = GetComponentInChildren<LookAtTarget>();
        animator = GetComponentInChildren<Animator>();
        player = controller.player;

        actionList.Add("Idle");
        actionList.Add("ShootPlayer");
        actionList.Add("Pushback");
    }

    //ACTIONS ---------------------

    //Action 0 - Idle
    public IEnumerator Idle()
    {
        yield return new WaitForSeconds(1);
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

        //Back to Default State
        lookAtTarget.isAiming = true;
        animator.SetTrigger(BossAnimation.Idle);
    }

    //Action 2 - Pushback player
    public IEnumerator Pushback()
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        yield return StartCoroutine(playerMove.knockBack(Vector2.down, pushbackIntensity));
    }


    public void ShootLaser(Vector3 target)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);


        

        laserManager.CreateLaser(lookAtTarget.aimAngle);
    }

    public void ShootLaser(Vector3 target, Laser laser)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        laserManager.CreateLaser(targetAngle, laser);
    }

    public Laser BigLaser()
    {
        Laser laser = new Laser
        {
            gainSpeed = (float)0.1,
            diminishSpeed = (float)0.1,
            holdTime = 5,
            maxWidth = 10,

            //indicateAttack = true,
            //indicatorTime = 3
        };

        return laser;
    }

    public override void DefaultState()
    {
        
    }
}
