using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Boss2_Actions))]
[RequireComponent(typeof(Boss2_Move))]
public class Boss2_Controller : _BossBase
{
    protected override void Init()
    {
        
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


    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
