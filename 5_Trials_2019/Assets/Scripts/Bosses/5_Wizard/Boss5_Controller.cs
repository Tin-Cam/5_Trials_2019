using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Controller : _BossBase
{
    public float bossLevel = 1;

    private Boss5_Action action;
    private Boss5_Move move;
    private Boss5_Commands command;
    private Boss5_Shield shield;

    protected override void Init()
    {
        action = GetComponent<Boss5_Action>();
        move = GetComponent<Boss5_Move>();
        command = GetComponent<Boss5_Commands>();
        shield = GetComponent<Boss5_Shield>();

        actionBase = action;
        moveBase = move;

        action.Init();
        move.Init();
        command.Init();
        shield.Init();
    }

    public override void BossHurt()
    {
        shield.BossHit();

        if (!shield.isShieldActive)
            TakeDamage(1);
        else
            audioManager.Play("Boss_NoDamage");
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
