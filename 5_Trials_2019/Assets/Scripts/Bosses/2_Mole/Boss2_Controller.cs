using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Boss2_Actions))]
[RequireComponent(typeof(Boss2_Move))]
public class Boss2_Controller : _BossBase
{
    private Boss2_Actions action;
    private Boss2_Move move;

    public int maxAction;
    private int vulCounter;
    public bool isHitable;

    protected override void Init()
    {
        action = GetComponent<Boss2_Actions>();
        move = GetComponent<Boss2_Move>();

        actionBase = action;
        moveBase = move;

        action.Init();
        move.Init();

        PickAction(0);
    }

    //AI -----------------------------------

    //Picks the next action to take. Runs after an action has been performed
    public void NextAction()
    {
        int rng = Random.Range(0, maxAction);

        //Checks if the picked action has already been used twice
        while (action.CheckLastAction(rng))
            rng = Random.Range(0, maxAction);

        //Forces the boss to become vulnerable if it hasn't been so after a few cycles
        if (rng == 0 | vulCounter >= 4)
        {
            vulCounter = 0;
            PickAction(0);
            return;
        }

        vulCounter++;
        PickAction(rng);
    }

    //REDUNDANT
    protected override void Act()
    {
       
    }

    //Boss will not take damage when underground or using desperation attack
    protected override void BossHurt()
    {
        if (isHitable)
            TakeDamage(1);
    }

    protected override void CheckHealth()
    {
        if (health <= maxHealth * 0.6 & phase < 1)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.25 & phase < 2)
        {
            IncreasePhase();
        }
    }

    protected override void IncreasePhase()
    {
        phase++;
        if (phase == 1)
            SetPhase_1();

        if (phase == 2)
            SetPhase_2();
    }

    private void SetPhase_1()
    {
        float newSpeed = move.digSpeed * 2;
        float newHold =  action.moveHoldTime * (float) 0.5;

        move.ChangeSpeed(newSpeed);
        action.moveHoldTime *= (float) 0.5;
        action.attackSplits = 2;
      
    }

    private void SetPhase_2()
    {
        float newSpeed = move.digSpeed * (float)1.5;
        float newHold = action.moveHoldTime * (float)0.75;

        move.ChangeSpeed(newSpeed);
        action.moveHoldTime *= (float)0.5;

        action.idleTime *= (float)0.66;
        action.attackHoldTime *= (float)0.66;

        DefaultState();
        PickAction(3);
        maxAction = 4;
    }


    protected override void StartDeath()
    {
        Die();
    }


    public override void DefaultState()
    {
        move.DefaultState();
        action.DefaultState();
        isHitable = true;
    }
}
