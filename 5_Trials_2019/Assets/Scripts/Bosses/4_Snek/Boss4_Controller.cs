using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Controller : _BossBase
{
    private Boss4_Move move;
    private Boss4_Action action;

    public int minIdle;
    public int maxIdle;


    protected override void Init()
    {
        move = GetComponent<Boss4_Move>();
        action = GetComponent<Boss4_Action>();

        moveBase = move;
        actionBase = action;

        move.Init();
        action.Init();

        StartCoroutine(Action());
    }

    public IEnumerator Action()
    {
        yield return move.StartToEnd();
        StartCoroutine(Action());
    }



    //Handles idleing
    public IEnumerator Idle()
    {
        //Wait Idle for some time
        float rngIdle = Random.Range(minIdle, (maxIdle + 1));
        float idleTimer = 0;

        while (idleTimer < rngIdle)
        {
            idleTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
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
