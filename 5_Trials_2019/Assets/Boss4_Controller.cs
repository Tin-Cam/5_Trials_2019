﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Controller : _BossBase
{
    protected override void Init()
    {
        
    }




    public override void BossHurt()
    {
        TakeDamage(1);
    }

    public override void DefaultState()
    {
        //isHitable = true;
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

    //REDUNDANT
    protected override void Act()
    {

    }
}
