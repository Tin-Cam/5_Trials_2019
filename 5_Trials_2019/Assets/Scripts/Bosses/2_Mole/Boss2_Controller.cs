using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Boss2_Actions))]
[RequireComponent(typeof(Boss2_Move))]
public class Boss2_Controller : _BossBase
{
    private Boss2_Actions action;
    private Boss2_Move move;

    protected override void Init()
    {
        action = GetComponent<Boss2_Actions>();
        move = GetComponent<Boss2_Move>();
    }

    void Update()
    {
        AI();
    }

    //AI -----------------------------------

    private void AI()
    {
        if (!hasAI)
            return;

        if (action.isActing)
            return;

        aiTimerCount++;

        if (aiTimerCount >= aiTimer)
        {
            Act();
            DefaultState();
        }
    }

    private void Act()
    {
        move.MovePosition();
    }

    protected override void BossHurt()
    {
        TakeDamage(1);
    }

    protected override void CheckHealth()
    {
        //Add Phases
    }

    protected override void IncreasePhase()
    {
        throw new System.NotImplementedException();
    }


    protected override void StartDeath()
    {
        Die();
    }


    

    public override void DefaultState()
    {
        SetAITimer();
    }
}
