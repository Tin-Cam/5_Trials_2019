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

    public bool isHitable = true;

    private Boss3_Actions action;

    protected override void Init()
    {
        action = GetComponent<Boss3_Actions>();
        
        action.Init();

        actionBase = action;

        //if (GameData.difficulty == 0)
        //    EasyMode();

        StartCoroutine(NextAction());
    }


    void OnDrawGizmos()
    {
        Vector3 pos = new Vector3( 3, 4, 0);
        Handles.Label(pos, "Retaliation: " + retaliateCounter + "/" + retaliateHitCount);        
    }

    public IEnumerator NextAction()
    {
        if (!hasAI)
            yield break;

        //Wait Idle for some time
        float rngIdle = Random.Range(minIdle, (maxIdle + 1));
        Debug.Log("Waiting for " + rngIdle + " seconds");
        yield return new WaitForSeconds(rngIdle);

        //Retaliate if possible
        if(retaliateCounter >= retaliateHitCount)
        {
            retaliateCounter = 0;
            StartCoroutine(action.SpreadShot());
            yield break;
        }

        //Pick an Attack
        int rngAction = Random.Range(0, maxAction);
        Debug.Log("Starting action " + rngAction);
        PickAction(rngAction);
    }


    

    //REDUNDANT
    protected override void Act()
    {
        
    }

    protected override void BossHurt()
    {
        if (isHitable)
        {
            retaliateCounter++;
            TakeDamage(1);
        }
        else
            audioManager.Play("Boss_NoDamage");
    }

    protected override void CheckHealth()
    {
        if (health <= maxHealth * 0.8 & phase < 1)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.25 & phase < 2)
        {
            IncreasePhase();
        }
    }

    protected override void IncreasePhase()
    {
        phase++;
        SetPhase();
    }

    private void SetPhase()
    {
        minIdle--;
        maxIdle--;

        //Changes Laser Properties
        float gain = action.GetStandardLaser().gainSpeed;
        float diminish = action.GetStandardLaser().diminishSpeed;
        float hold = action.GetStandardLaser().holdTime;
        float width = action.GetStandardLaser().maxWidth;

        gain *= 1.2f;
        diminish *= 1.2f;
        hold *= 0.8f;
        width *= 1.5f;

        action.indicateTime *= 0.8f;

        action.SetStandardLaser(gain, diminish, hold, width);


        if (phase != 2)
            return;

        int subtraction = action.spreadShotSubtraction - 1;

        action.SetSpreadShotSubtraction(subtraction);

        StopAllCoroutines();
        action.StopActing();
        PickAction(2);
        maxAction = 3;
    }

    private void EasyMode()
    {
        health -= 5;
        maxHealth -= 5;
        healthBar.initHealth(health);

        minIdle++;
        maxIdle++;

        //Changes Laser Properties
        float gain = action.GetStandardLaser().gainSpeed;
        float diminish = action.GetStandardLaser().diminishSpeed;
        float hold = action.GetStandardLaser().holdTime;
        float width = action.GetStandardLaser().maxWidth;

        gain *= 0.8f;
        diminish *= 0.8f;
        hold *= 0.2f;
        width *= 0.9f;

        action.indicateTime *= 1.2f;

        action.SetStandardLaser(gain, diminish, hold, width);
    }

    public override void DefaultState()
    {
        isHitable = true;
    }

    protected override void StartDeath()
    {
        Die();
    }
}
