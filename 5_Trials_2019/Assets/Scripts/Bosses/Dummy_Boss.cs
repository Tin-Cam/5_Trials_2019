using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Boss : _BossBase
{
    protected override void Init()
    {
        actionBase = null;
        moveBase = null;
    }
    

    protected override void BossHurt()
    {
        TakeDamage(1);
    }

    protected override void StartDeath()
    {
        Destroy(this.gameObject);
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckHealth()
    {

    }

    protected override void IncreasePhase()
    {
        throw new System.NotImplementedException();
    }

}
