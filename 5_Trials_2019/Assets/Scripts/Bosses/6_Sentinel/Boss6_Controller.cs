using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Controller : _BossBase
{
    public float bossLevel = 1;
    public Animator bossAnimator;

    private Boss6_Action action;
    private Boss6_Move move;
    private Boss6_Commands command;

    protected override void Init()
    {
        action = GetComponent<Boss6_Action>();
        move = GetComponent<Boss6_Move>();
        command = GetComponent<Boss6_Commands>();

        actionBase = action;
        moveBase = move;

        action.Init();
        move.Init();
        command.Init();
    }

    public override void BossHurt()
    {
        TakeDamage(1);
    }

    public override void DefaultState()
    {
        
    }

    protected override void Act()
    {
        throw new System.NotImplementedException();
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

}
