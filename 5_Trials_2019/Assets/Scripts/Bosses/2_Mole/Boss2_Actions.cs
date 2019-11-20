using System.Collections;
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
    [Space(15)]
    public int attackSplits = 1;
    public int shotAmount;
    public float doubleOffset;
    public float tripleOffset;
    public float attackHoldTime;
    public float desperationTime;
    [Space(15)]
    public float moveHoldTime;
    public int rapidMoveAmount;
    

    public void Init()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<Boss2_Controller>();
        move = GetComponent<Boss2_Move>();

        player = controller.player;
        isActing = false;

        actionList.Add("Idle");

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
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds((float) 0.5);

        Vector3 target = player.transform.position;

        int rng = Random.Range(0, attackSplits);

        for (int i = 0; i < shotAmount; i++)
        {
            if (rng == 0)
                TripleShot(target);
            else
                DoubleShot(target);
            yield return new WaitForSeconds((float)0.2);
        }

        yield return new WaitForSeconds(attackHoldTime);
        
        StartCoroutine(MoveRandom());
    }

    //Action 2 - Rapid Attack
    private IEnumerator RapidAttack()
    {
        float speed = move.digSpeed;

        move.ChangeSpeed(speed * 2);    

        for (int i = 0; i < rapidMoveAmount; i++)
        {
            yield return move.MovePosition();
            animator.SetTrigger("Attack");
            yield return RapidShot(shotAmount);
            yield return new WaitForSeconds((float)0.2);
        }

        move.ChangeSpeed(speed);
        StartCoroutine(MoveRandom());
    }

    //Action 3 - Desperation (Cannot be damaged during attack)
    private IEnumerator Desperation()
    {
        yield return move.MovePosition(4);
        animator.SetTrigger("Desperation");
        controller.isHitable = false;

        laser.gameObject.SetActive(true);
        yield return laser.StartSpin();
        yield return new WaitForSeconds(desperationTime);
        yield return laser.EndSpin();

        yield return new WaitForSeconds(1);
        controller.isHitable = true;
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
    private IEnumerator RapidShot(int amount)
    {
        int rng = Random.Range(0, attackSplits);
        Vector3 target = player.transform.position;

        for (int i = 0; i < amount; i++)
        {
            if (rng == 0)
                Shoot(0, target);
            else
                DoubleShot(target);
            yield return new WaitForSeconds((float)0.1);
        }
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
        laser.EndSpin();
    }
}
