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

    protected override void Init()
    {
        action = GetComponent<Boss6_Action>();
        move = GetComponent<Boss6_Move>();
        command = GetComponent<Boss6_Commands>();

        actionBase = action;
        moveBase = move;

        action.Init();
        move.Init();
        command.Init();
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
        }

        if (health <= maxHealth * 0.4 & phase < 4)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.10 & phase < 5)
        {
            IncreasePhase();
        }

        // if (health <= maxHealth * 0.1 & phase < 6)
        // {
        //     IncreasePhase();
        // }
    }

    protected override void IncreasePhase()
    {
        audioManager.Play("Boss_Hit", 1, 1.5f);
        healthBar.FlashBar();
        phase++;
        bossLevel += bosslevelIncrements;
        command.ChangeCommandList(phase);
    }

    protected override void StartDeath()
    {
        Die();
    }

}
