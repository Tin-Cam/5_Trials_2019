  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Action : _ActionBase
{
    public bool desperation = false;

    public float attackSpeed;
    public int shootCycleCount;
    public float shootRate;
    public float shootMaxDelay;

    public int bombCycleCount;
    public float bombRate;

    public int maxAttackRNG;
    private int defaultAttackRNG;


    public int desperationCount;
    private int currentDesperationCount;

    [HideInInspector]
    public int desperationChance;
    public int maxDespChanceCounter;


    private SnakeMovement head;

    private Boss4_Controller controller;
    private Boss4_Move move;
    private GameObject player;

    public void Init(SnakeMovement head)
    {
        controller = GetComponent<Boss4_Controller>();
        move = GetComponent<Boss4_Move>();

        this.head = head;
        player = controller.player;

        SetAttackSpeed(attackSpeed);

        actionList.Add("DesperationMode");
        actionList.Add("Shoot");
        actionList.Add("LayBomb");
    }

    public void DecideActions()
    {
        int rng;

        //Make Desperation SFX if currently desperarte
        if(desperation){
            //yield return WaitForMoveCount(3);
        }

        //Decide if desperate
        if (!desperation)
        {
            rng = Random.Range(0, maxDespChanceCounter);
            Debug.Log("RNG: " + rng + " < Chance: " + desperationChance + "------" + (rng < desperationChance));
            if (rng < desperationChance)
            {
                StartCoroutine(PrepareDesperation());
                return;
            }
        }

        //Decide if attacking
        rng = Random.Range(0, maxAttackRNG);
        Debug.Log("RNG is :" + rng);
        switch (rng)
        {
            //Shoot
            case 0:
                StartCoroutine(PrepareShoot());
                break;
            case 1:
                StartCoroutine(PrepareShoot());
                break;

            //Bomb
            case 2:
                StartCoroutine(PrepareBomb());
                break;
            case 3:
                StartCoroutine(PrepareBomb());
                break;

            //Shoot and Bomb
            case 4:
                StartCoroutine(PrepareShoot());
                StartCoroutine(PrepareBomb());
                break;

            default:
                Debug.LogError("Unexpected attack RNG value: " + rng);
                break;

        }        

    }

    public IEnumerator PrepareShoot()
    {
        yield return WaitForMoveCount(2);

        //Calculates the delay of attack
        float rng = Random.Range(1, shootMaxDelay);
        yield return new WaitForSeconds(rng);

        StartCoroutine(Shoot());
    }

    public IEnumerator PrepareBomb()
    {
        yield return WaitForMoveCount(3);
        StartCoroutine(LayBomb());
    }

    public IEnumerator PrepareDesperation()
    {
        yield return WaitForMoveCount(2);
        StartCoroutine(DesperationMode());
    }

    private IEnumerator WaitForMoveCount(int targetCount)
    {
        while(move.moveCount < targetCount)
            yield return new WaitForFixedUpdate();
    }

    public IEnumerator DesperationMode()
    {
        if (desperation)
            yield break;

        ShowDesperationFilter(true);

        currentDesperationCount = desperationCount;

        move.SetSpeed(2);
        yield return new WaitForSeconds(1);

        desperation = true;
        move.SetSpeed(move.GetDefaultSpeed() * 3);
        AudioManager.instance.Play("Boss5_Desp1", 0.75f, 1.5f);

        foreach(Transform bodyPart in head.body)
        {
            Animator animator = bodyPart.GetComponent<Animator>();
            animator.SetBool("Desperation", true);
        }
    }

    public void ReduceDesperation()
    {
        currentDesperationCount--;
        if (currentDesperationCount <= 0)
            ExitDesperationMode();
    }

    private void ExitDesperationMode()
    {
        ShowDesperationFilter(false);
        move.ResetSpeed();
        desperation = false;
        desperationChance = 0;
        foreach (Transform bodyPart in head.body)
        {
            Animator animator = bodyPart.GetComponent<Animator>();
            animator.SetBool("Desperation", false);
        }
    }


    public IEnumerator Shoot()
    {
        int counter = 0;

        //Cycles through each segment (except the last one) and attempts to shoot it
        while (counter < shootCycleCount)
        {
            for (int i = 1; i < head.body.Count; i++)
            {
                Segment segment = head.body[i - 1].GetComponent<Segment>();
                if (!segment.IsDestroyed())
                {
                    yield return segment.StartShoot(player.transform.position);
                    counter++;
                    yield return new WaitForSeconds(shootRate);
                }
            }
        }
    }

    public IEnumerator LayBomb()
    {
        for (int i = 0; i < bombCycleCount; i++)
        {
            Segment segment = head.body[head.body.Count - 1].GetComponent<Segment>();
            if (!segment.CheckShootBounds())
                yield break;
            segment.Shoot(player.transform.position);
            yield return new WaitForSeconds(bombRate);
        }
    }

    public void SetAttackSpeed(float speedMultiplier)
    {
        foreach (Transform bodyPart in head.body)
        {
            Animator animator = bodyPart.GetComponent<Animator>();
            animator.SetFloat("Attack Speed", speedMultiplier);
        }
    }

    public bool CheckSegmentBounds(int segmentNO)
    {
        Segment segment = head.body[segmentNO].GetComponent<Segment>();
        return segment.CheckShootBounds();
    }


    public override void DefaultState()
    {
        ExitDesperationMode();
    }
}
