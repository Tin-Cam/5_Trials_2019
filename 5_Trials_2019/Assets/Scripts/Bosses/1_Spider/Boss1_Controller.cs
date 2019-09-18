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
    private Vector2Int last2Actions;

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

        aiTimer++;

        if (aiTimer >= 1000)
        {
            phase1();
        }
    }

    

    void phase1()
    {
        int rng = Random.Range(0, maxAction);

        while (checkLastAction(rng))
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

    override protected void increasePhase()
    {

    }

    override protected void checkHealth()
    {
        if (health <= maxHealth * 0.6 & phase < 1)
        {
            move.speed *= 2;
            eyes.setMiniEyeTimer(1000);
            phase++;
        }
        if (health <= maxHealth * 0.25 & phase < 2)
        {
            move.speed *= 2;
            eyes.setMiniEyeTimer(500);
            maxAction = 4;
            phase++;
            StopAction();
            PickAction(3);
        }
    }

    //Ensures no action is used 3 times in a row
    bool checkLastAction(int action)
    {
        int actionSum = action * 2;
        int vecSum = last2Actions.x + last2Actions.y;

        if (actionSum == vecSum)
            return true;

        last2Actions.y = last2Actions.x;
        last2Actions.x = action;

        return false;
    }


    protected override void death()
    {
        Destroy(eyes.miniEyeL.gameObject);
        Destroy(eyes.miniEyeR.gameObject);
        Destroy(this.gameObject);
    }


    protected override void bossHurt()
    {
        if (eyes.isEyeOpen)
            takeDamage(1);
    }

    public override void DefaultState()
    {
        aiTimer = 0;

        action.DefaultState();
        move.DefaultState();
        eyes.DefaultState();
    }
}
