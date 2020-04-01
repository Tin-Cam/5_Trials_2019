using System.Collections;
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

public class StandardLaser
{
    Laser laser;

    public float gainSpeed = 0.2f;
    public float diminishSpeed = 0.2f;
    public float holdTime = 1;
    public float maxWidth = 1;

    public StandardLaser(Laser reference)
    {
        laser = reference;
    }

    public void SetLaser()
    {
        laser.gainSpeed = gainSpeed;
        laser.diminishSpeed = diminishSpeed;
        laser.holdTime = holdTime;
        laser.maxWidth = maxWidth;
    }

    public void SetLaser(float gain, float diminish, float hold, float width)
    {
        gainSpeed = gain;
        diminishSpeed = diminish;
        holdTime = hold;
        maxWidth = width;

    }

    public Laser GetLaser()
    {
        SetLaser();
        return laser;
    }
}


public class Boss3_Actions : _ActionBase
{
    //public Laser laser;

    private Boss3_Controller controller;
    private LaserManager laserManager;
    private LookAtTarget lookAtTarget;
    private Animator animator;
    private GameObject player;
    private StandardLaser stLaser;
    private RockLaser currentRockLaser;
    private SpreadShot currentSpreadShot;

    public Laser laserRef;
    public float indicateTime;

    public RockLaser rockLaser;

    public SpreadShot spreadShot;
    public int spreadShotSubtraction;

    public GameObject dustCloud;
    public float pushbackIntensity;

    public GameObject[] desperationTargets;


    public void Init()
    {
        controller = GetComponent<Boss3_Controller>();
        laserManager = GetComponent<LaserManager>();
        lookAtTarget = GetComponentInChildren<LookAtTarget>();
        animator = GetComponent<Animator>();
        player = controller.player;

        stLaser = new StandardLaser(laserRef);

        SetSpreadShotSubtraction(spreadShotSubtraction);

        //actionList.Add("Idle");
        actionList.Add("ShootPlayer");
        //actionList.Add("Retaliate");
        actionList.Add("RockLaserAttack");
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
        yield return laserManager.IndicateLaser(indicateTime, angle);
        
        //Fire
        animator.SetTrigger(BossAnimation.Fire);
        yield return laserManager.ShootLaser(lookAtTarget.aimAngle, stLaser.GetLaser());

        DefaultState();
    }

    //Action 2 - Retaliate
    public IEnumerator Retaliate()
    {
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        animator.SetTrigger(BossAnimation.Retaliate);

        yield return new WaitForEndOfFrame();
        yield return WaitForAnimation("Boss3_Retaliate");
    }

    //Action 2.1 - Pushback player
    public void Pushback()
    {
        Instantiate(dustCloud, transform);
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        StartCoroutine(playerMove.knockBack(Vector2.down, pushbackIntensity));
    }

    //Action 2.2 - Retaliate after pushback (using a spreadshot)
    public IEnumerator SpreadShot()
    {
        RemoveExcess();
        yield return Retaliate();

        animator.SetTrigger(BossAnimation.AttackStandard);
        yield return new WaitForSeconds(1);       
     
        animator.SetTrigger(BossAnimation.Fire);
        spreadShot.transform.position = new Vector3(0, -1, 0);
        currentSpreadShot = Instantiate(spreadShot, transform);
        yield return new WaitForSeconds(2);
        DefaultState();
    }
 
    //Action 3 - Rock Laser Attack
    public IEnumerator RockLaserAttack()
    {
        currentRockLaser = Instantiate(rockLaser, transform);

        //Randomise direction to move Rock Laser
        currentRockLaser.isMirror = (Random.value > 0.5f);

        DefaultState();
        yield return new WaitForSeconds(2);
    }

    //Action 4 - Desperation Attack
    public IEnumerator DesperationAttack()
    {
        yield return Retaliate();

        RemoveExcess();

        //Pick a target to aim at
        Transform target = desperationTargets[Random.Range(0, desperationTargets.Length)].transform;

        //Charge Attack
        ShowDesperationFilter(true);
        lookAtTarget.ChangeTarget(target);
        animator.SetTrigger(BossAnimation.AttackDesperation);
        controller.isHitable = false;
        yield return new WaitForSeconds(2);

        //Shoot
        Quaternion targetAngle = CalculateAim(target.position);
        animator.SetTrigger(BossAnimation.Fire);
        yield return laserManager.ShootLaser(targetAngle, BigLaser());

        ShowDesperationFilter(false);
        DefaultState();
    }

    //Removes rock lasers or spreadshots to avoid the player getting stuck
    public void RemoveExcess()
    {
        try
        {
            Destroy(currentRockLaser.gameObject);
            Destroy(currentSpreadShot.gameObject);
        }
        catch (MissingReferenceException)
        {

        }
        catch (System.NullReferenceException)
        {

        }
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

    public StandardLaser GetStandardLaser()
    {
        return stLaser;
    }

    public void SetStandardLaser(float gain, float diminish, float hold, float width)
    {
        stLaser.SetLaser(gain, diminish, hold, width);
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

    public void SetSpreadShotSubtraction(int subtraction)
    {
        if (subtraction < 1)
            subtraction = 1;

        spreadShotSubtraction = subtraction;
        spreadShot.subtraction = spreadShotSubtraction;
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
        controller.DefaultState();
        animator.SetTrigger(BossAnimation.Idle);
        lookAtTarget.isAiming = true;
        lookAtTarget.ChangeTarget(player.transform);
        StartCoroutine(controller.NextAction());
    }
}
