using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Boss : _BossBase
{
    protected override void Init()
    {
        actionBase = null;
        moveBase = null;

        hasAI = false;
    }
    

    public override void BossHurt()
    {
        TakeDamage(1);
    }

    protected override void StartDeath()
    {
        Die();
    }

    public override void DefaultState()
    {

    }

    protected override void CheckHealth()
    {

    }

    protected override void IncreasePhase()
    {

    }

    protected override void Act()
    {
        
    }
}
