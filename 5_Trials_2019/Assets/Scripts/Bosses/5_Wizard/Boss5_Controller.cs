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

        action.Init();
        move.Init();


    }

    public override void BossHurt()
    {
        throw new System.NotImplementedException();
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }

    protected override void Act()
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckHealth()
    {
        throw new System.NotImplementedException();
    }

    protected override void IncreasePhase()
    {
        throw new System.NotImplementedException();
    }   

    protected override void StartDeath()
    {
        throw new System.NotImplementedException();
    }
}
