using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Boss3_Actions))]
public class Boss3_Controller : _BossBase
{
    public int maxAction;
    public int minIdle;
    public int maxIdle;

    public int retaliateHitCount;
    private int retaliateCounter;

    private Boss3_Actions action;

    protected override void Init()
    {
        action = GetComponent<Boss3_Actions>();
        
        action.Init();

        actionBase = action;

        StartCoroutine(NextAction());
    }


    void OnDrawGizmos()
    {
        Vector3 pos = new Vector3( 3, 4, 0);

        Handles.Label(pos, "Retaliation: " + retaliateCounter + "/" + retaliateHitCount);

        
    }

    public IEnumerator NextAction()
    {
        //Wait Idle for some time
        float rngIdle = Random.Range(minIdle, (maxIdle + 1));
        Debug.Log("Waiting for " + rngIdle + " seconds");
        yield return new WaitForSeconds(rngIdle);

        //Retaliate if possible
        if(retaliateCounter >= retaliateHitCount)
        {
            retaliateCounter = 0;
            StartCoroutine(action.Retaliate());
            yield break;
        }

        //Pick an Attack
        int rngAction = Random.Range(0, maxAction);
        Debug.Log("Starting action " + rngAction);
        action.StartAction(rngAction);
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
        retaliateCounter++;
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
