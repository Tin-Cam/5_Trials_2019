using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Controller : _BossBase
{
    private Boss4_Move move;
    private Boss4_Action action;

    [Range(0, 10)] public int idleProbability;
    public int maxNoIdleCount;
    private int noIdleCounter;

    public SnakeMovement head;

    protected override void Init()
    {
        move = GetComponent<Boss4_Move>();
        action = GetComponent<Boss4_Action>();

        moveBase = move;
        actionBase = action;

        foreach(Transform bodyPart in head.body)
        {
            Segment segment = bodyPart.GetComponent<Segment>();
            segment.SetController(this);
            segment.SetAction(action);
        }

        move.Init(head);
        action.Init(head);

        if (GameData.difficulty == 0)
            EasyMode();

        StartCoroutine(StartCycle());
    }

    public IEnumerator Action()
    {
        yield return move.StartToEnd();
        StartCoroutine(Action());
    }

    public IEnumerator StartCycle()
    {
        int rng;

        //Decide if attacking
        rng = Random.Range(0, 10);
        if (rng > idleProbability - 1 && noIdleCounter < maxNoIdleCount)
        {
            //Decide Action
            action.DecideActions();
            noIdleCounter++;
        }
        else
        {
            noIdleCounter = 0;
        }
        

        if (action.desperation)
            action.ReduceDesperation();

        //Decide how to move
        rng = Random.Range(0, 2);
        if (action.desperation)
            rng += 1;

        switch(rng){
            case 0:
                yield return move.StartToEnd();
                break;
            case 1:
                yield return move.StartToMiddleToEnd();
                break;
            case 2:
                yield return move.CircleMiddle();
                break;
            default:
                yield return move.StartToEnd();
                break;
        }
        if (phase >= 2)
            action.desperationChance +=1;
        StartCoroutine(StartCycle());
    }

    //Not used
    public override void BossHurt()
    {
        
    }

    public void SegmentHurt()
    {
        if(phase >= 2)
            action.desperationChance++;
        TakeDamage(1);
    }

    public override void DefaultState()
    {
        //isHitable = true;
    }

    

    protected override void CheckHealth()
    {
        if (health <= maxHealth * 0.8 & phase < 1)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.5 & phase < 2)
        {
            IncreasePhase();
        }
    }

    protected override void IncreasePhase()
    {
        phase++;
        move.HardSetSpeed(move.GetDefaultSpeed() + 1);

        action.bombCycleCount++;
        action.shootCycleCount += 3;

        idleProbability--;

        action.attackSpeed += 0.5f;
        action.SetAttackSpeed(action.attackSpeed);

        if (phase >= 2)
            StartCoroutine(action.DesperationMode());
    }

    private void EasyMode()
    {
        //return;
        health -= 4;
        maxHealth -= 4;
        healthBar.initHealth(health);

        action.bombCycleCount--;
        action.shootCycleCount -= 3;
        idleProbability += 2;

        action.attackSpeed -= 0.5f;
        action.SetAttackSpeed(action.attackSpeed);
    }

    protected override void StartDeath()
    {
        //Explosions for bodyparts
        for (int i = 1; i < head.body.Count; i++)
            Instantiate(deathExplosion, head.body[i].position, transform.rotation);
        //Explosion for head (AKA moving the controller to the head's position so that the explosion occurs at that positon instead <insert galaxy brain>)
        transform.position = head.transform.position;

        Die();
    }

    //REDUNDANT
    protected override void Act()
    {

    }
}
