using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Controller : _BossBase
{
    public float bossLevel = 1;
    public float bosslevelIncrements = 0.2f;
    public Animator bossAnimator;

    private Boss6_Action action;
    private Boss6_Move move;
    private Boss6_Commands command;
    private SmokeController smoke;

    protected override void Init()
    {
        action = GetComponent<Boss6_Action>();
        move = GetComponent<Boss6_Move>();
        command = GetComponent<Boss6_Commands>();
        smoke = GetComponent<SmokeController>();

        actionBase = action;
        moveBase = move;

        if (FlagManager.instance.easyMode)
            EasyMode();

        action.Init();
        move.Init();
        command.Init();

        //Hides Sentinel's healthbar if playing story mode
        //if(FlagManager.instance.storyMode)
        //    healthBar.SetBarHider(true);
    }

    public override void BossHurt()
    {
        TakeDamage(1);
        command.IncrementDespCount();
        CheckHealth();
    }

    public override void DefaultState()
    {
        
    }

    protected override void Act()
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckHealth()
    {
        int phaseAmount = 7;
        float phaseDivision = 1 / phaseAmount;

        int smokeCount = smoke.smokeCount;

        if (health <= maxHealth * 0.9 & phase < 1)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.8 & phase < 2)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.6 & phase < 3)
        {
            IncreasePhase();
            smoke.SetSmoke(smokeCount + 1);
        }

        if (health <= maxHealth * 0.4 & phase < 4)
        {
            IncreasePhase();
            smoke.SetSmoke(smokeCount + 1);
        }

        if (health <= maxHealth * 0.10 & phase < 5)
        {
            IncreasePhase();
            smoke.SetSmoke(smokeCount + 1);
        }

        // if (health <= maxHealth * 0.1 & phase < 6)
        // {
        //     IncreasePhase();
        // }
    }

    private void EasyMode(){
        health -= 20;
        maxHealth -= 20;
        bossLevel -= 0.3f;
    }

    protected override void IncreasePhase()
    {
        audioManager.Play("Boss_Hit", 1, 1.5f);
        healthBar.ShakeBar();
        phase++;
        bossLevel += bosslevelIncrements;
        command.ChangeCommandList(phase);
    }

    public override void StartDeath()
    {
        command.StopCommand();
        Die();
    }

}
