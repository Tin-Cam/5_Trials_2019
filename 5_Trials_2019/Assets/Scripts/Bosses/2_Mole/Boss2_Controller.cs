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

    protected override void Init()
    {
        action = GetComponent<Boss2_Actions>();
        move = GetComponent<Boss2_Move>();

        actionBase = action;
        moveBase = move;

        action.Init();
        move.Init();

    }

    void Update()
    {
        //AI();
    }

    //AI -----------------------------------

    //Picks the next action to take. Runs after an action has been performed
    public void NextAction()
    {
        int rng = Random.Range(0, maxAction);

        while (action.CheckLastAction(rng))
            rng = Random.Range(0, 3);

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

    protected override void Act()
    {
       
    }

    protected override void BossHurt()
    {
        TakeDamage(1);
    }

    protected override void CheckHealth()
    {
        if (health <= maxHealth * 0.6 & phase < 1)
        {
            IncreasePhase();
        }
    }

    protected override void IncreasePhase()
    {
        phase++;
        if (phase == 1)
            SetPhase_1();
    }

    private void SetPhase_1()
    {
        float newSpeed = move.digSpeed * 2;
        float newHold =  action.moveHoldTime * (float) 0.5;

        move.ChangeSpeed(newSpeed);
        action.moveHoldTime *= (float) 0.5; 
    }


    protected override void StartDeath()
    {
        Die();
    }


    

    public override void DefaultState()
    {
        move.DefaultState();
        action.DefaultState();
    }
}
