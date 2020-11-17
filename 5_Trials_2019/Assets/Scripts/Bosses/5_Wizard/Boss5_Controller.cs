using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Controller : _BossBase
{
    public float bossLevel = 1;
    public Animator bossAnimator;

    private Boss5_Action action;
    private Boss5_Move move;
    private Boss5_Commands command;
    private Boss5_Shield shield;

    public CM_Boss5 cM_Boss5;

    protected override void Init()
    {
        action = GetComponent<Boss5_Action>();
        move = GetComponent<Boss5_Move>();
        command = GetComponent<Boss5_Commands>();
        shield = GetComponent<Boss5_Shield>();

        actionBase = action;
        moveBase = move;

        if (FlagManager.instance.easyMode)
            EasyMode();


        action.Init();
        move.Init();
        command.Init();
        shield.Init();
    }

    public override void BossHurt()
    {
        shield.BossHit();
        command.IncrementDespCount();

        if (!shield.isShieldActive)
            TakeDamage(1);
        else
            audioManager.Play("Boss_NoDamage");
    }

    protected override void CheckHealth()
    {
        if (health <= maxHealth * 0.9 & phase < 1)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.75 & phase < 2)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.55 & phase < 3)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.35 & phase < 4)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.20 & phase < 5)
        {
            IncreasePhase();
        }

        if (health <= maxHealth * 0.10 & phase < 6)
        {
            IncreasePhase();
        }

    }

    private void EasyMode(){
        
    }

    protected override void IncreasePhase()
    {
        phase++;
        command.ChangeCommandList(phase);

        bossLevel += 0.2f;
    }

    public override void StartDeath()
    {
        //Maybe make the death look cooler
        cM_Boss5.Ending();
        Die();
    }

    public override void DefaultState()
    {
        
    }   

    //Not Used
    protected override void Act()
    {
        throw new System.NotImplementedException();
    }
   
}
