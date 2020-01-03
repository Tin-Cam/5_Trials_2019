using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boss3_Actions))]
public class Boss3_Controller : _BossBase
{
    private Boss3_Actions action;

    protected override void Init()
    {
        action = GetComponent<Boss3_Actions>();
        action.Init();

        actionBase = action;
        
    }

    public override void DefaultState()
    {
        
    }

    //REDUNDANT
    protected override void Act()
    {
        
    }

    protected override void BossHurt()
    {
        TakeDamage(1);
    }

    protected override void CheckHealth()
    {
        
    }

    protected override void IncreasePhase()
    {
        throw new System.NotImplementedException();
    }

    protected override void StartDeath()
    {
        Die();
    }
}
