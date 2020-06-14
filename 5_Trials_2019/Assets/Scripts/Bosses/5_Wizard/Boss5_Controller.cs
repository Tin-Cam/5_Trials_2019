using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Controller : _BossBase
{
    private Boss5_Action action;
    private Boss5_Controller controller;
    private Boss5_Move move;

    protected override void Init()
    {
        action = GetComponent<Boss5_Action>();
        move = GetComponent<Boss5_Move>();

        actionBase = action;
        moveBase = move;

        action.Init();
        move.Init();


    }

    public override void BossHurt()
    {
        TakeDamage(1);
    }

    protected override void CheckHealth()
    {

    }

    protected override void IncreasePhase()
    {

    }

    protected override void StartDeath()
    {
        Die();
    }

    public override void DefaultState()
    {
        
    }   

    //Not Used
    protected override void Act()
    {
        throw new System.NotImplementedException();
    }
}
