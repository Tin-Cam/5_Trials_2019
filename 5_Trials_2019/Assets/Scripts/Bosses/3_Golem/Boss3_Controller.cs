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
    private float idleTimer;

    public int retaliateHitCount;
    private int retaliateCounter;

    public bool isHitable = true;
    public bool forceDesperation;

    private Boss3_Actions action;

    protected override void Init()
    {
        action = GetComponent<Boss3_Actions>();
        
        action.Init();

        actionBase = action;

        if (FlagManager.instance.easyMode)
            EasyMode();

        StartCoroutine(NextAction());
    }

    public IEnumerator NextAction()
    {
        
        if (!hasAI)
            yield break;

        Debug.Log("Starting next action");

        if(forceDesperation){
            forceDesperation = false;
            PickAction(2);          
            yield break;
        }

        //Retaliate if possible
        if(retaliateCounter >= retaliateHitCount)
        {
            retaliateCounter = 0;
            StartCoroutine(action.SpreadShot());
            yield break;
        }

        yield return Idle();

        //Pick an Attack
        int rngAction = Random.Range(0, maxAction);

        //Ensures an action isn't used twice in a row
        while (action.CheckLastAction(rngAction))
            rngAction = Random.Range(0, maxAction);

        Debug.Log("Starting action " + rngAction);
        PickAction(rngAction);
    }

    //Handles Idleing. Hitting the boss increases the timer 
    public IEnumerator Idle()
    {
        //Wait Idle for some time
        float rngIdle = Random.Range(minIdle, (maxIdle + 1));
        idleTimer = 0;

        while(idleTimer < rngIdle)
        {
            idleTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    

    

    public override void BossHurt()
    {
        if (isHitable)
        {
            idleTimer++;
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
        audioManager.Play("Boss_Hit", 1, 1.5f);
        healthBar.ShakeBar();

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

        int subtraction = action.spreadShotSubtraction - 1;
        action.SetSpreadShotSubtraction(subtraction);

        //Only applies on Phase 2
        if (phase != 2)
            return;

        //action.DefaultState();

        //StopCoroutine(NextAction());
        //StopCoroutine(Idle());
        //action.StopActing();
        //PickAction(2);
        forceDesperation = true;
        maxAction = 3;
    }

    private void EasyMode()
    {
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
        width *= 0.9f;

        action.indicateTime *= 1.2f;

        action.SetStandardLaser(gain, diminish, hold, width);
    }

    public override void DefaultState()
    {
        isHitable = true;
    }

    public override void StartDeath()
    {
        Die();
    }

    //REDUNDANT
    protected override void Act()
    {

    }
}
