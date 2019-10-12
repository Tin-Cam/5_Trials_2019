using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Boss1_Actions))]
[RequireComponent(typeof(Boss1_Eyes))]
[RequireComponent(typeof(Boss1_Move))]
public class Boss1_Controller : _BossBase
{
    private Boss1_Actions action;
    private Boss1_Move move;
    private Boss1_Eyes eyes;

    //AI Variables
    public int maxAction;

    
    private int vulCounter;
    

    // Start is called before the first frame update
    protected override void Init()
    {
        action = GetComponent<Boss1_Actions>();
        move = GetComponent<Boss1_Move>();
        eyes = GetComponent<Boss1_Eyes>();

        actionBase = action;
        moveBase = move;

        action.Init();
        move.Init();
        eyes.Init();
    }



    // Update is called once per frame
    void Update()
    {
        AI();
    }

    //AI------------------------------------

    void AI()
    {
        if (!hasAI)
            return;

        if (action.isActing)
            return;

        aiTimerCount++;

        if (aiTimerCount >= aiTimer)
        {
            Act();
        }
    }

    

    void Act()
    {
        int rng = Random.Range(0, maxAction);

        while (action.CheckLastAction(rng))
            rng = Random.Range(0, 3);

        //Forces the boss to become vulnerable if it hasn't been so after a few cycles
        if (rng == 0 | vulCounter >= 4)
        {
            vulCounter = 0;
            PickAction(0);
            return;
        }

        vulCounter++;
        PickAction(rng);
    }

    override protected void CheckHealth()
    {
        if (health <= maxHealth * 0.6 & phase < 1)
        {
            IncreasePhase();
        }
        if (health <= maxHealth * 0.25 & phase < 2)
        {
            IncreasePhase();
        }
    }

    override protected void IncreasePhase()
    {
        phase++;
        if (phase == 1)
            SetPhase_1();
        if (phase >= 2)
            SetPhase_2();
    }

    private void SetPhase_1()
    {
        move.speed *= 2;
        eyes.setMiniEyeTimer(500);

        action.shortAttackAmount += 2;
        action.longAttackAmount += 5;
    }

    private void SetPhase_2()
    {
        move.speed *= 2;
        eyes.setMiniEyeTimer(250);
        maxAction = 4;

        action.shortAttackFrequency *= (float) 0.5;
        action.exposeEyeTime -= 1;

        StopAction();
        PickAction(3);
    }


    protected override void StartDeath()
    {
        Destroy(eyes.miniEyeL.gameObject);
        Destroy(eyes.miniEyeR.gameObject);
        eyes.enabled = false;
        animator.Play("Death");       
    }

    protected override void BossHurt()
    {
        if (eyes.isEyeOpen)
            TakeDamage(1);
    }


    public override void DefaultState()
    {
        SetAITimer();

        action.DefaultState();
        move.DefaultState();
        eyes.DefaultState();
    }
}
