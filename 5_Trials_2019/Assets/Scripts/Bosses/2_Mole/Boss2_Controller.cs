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


    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
