using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Commands : MonoBehaviour
{
    public float idleTime;

    public float actionPause;
    public float desperationPause;

    public int justSpinShootAmount;
    public int spinShootAndMoveAmount;

    private Boss5_Action action;
    private Boss5_Move move;
    private Boss5_Controller controller;
    private Boss5_Shield shield;

    public bool randomCommands;
    private List<string> commandList = new List<string>();
    private Queue<string> commandQueue = new Queue<string>();
    private int nextCommandNumber;

    public int maxDespCount;
    private int despCounter;
    private bool despCounterActive;
    

    // Start is called before the first frame update
    public void Init()
    {
        action = GetComponent<Boss5_Action>();
        move = GetComponent<Boss5_Move>();
        controller = GetComponent<Boss5_Controller>();
        shield = GetComponent<Boss5_Shield>();

        ChangeCommandList(0);

        StartCoroutine(NextCommand());
    }

    public IEnumerator NextCommand()
    {
        ResetAnimation();

        if (commandQueue.Count != 0)
        {
            //Uses queued command if any
            yield return StartCoroutine(commandQueue.Dequeue());
        }
        else
        {
            //Picks a command if nothing is queued
            nextCommandNumber = NextCommandNumber();
            yield return StartCoroutine(commandList[nextCommandNumber]);
        }

        ResetAnimation();

        //Halves idling time if shield is active
        if (!shield.isShieldActive)
            yield return new WaitForSeconds(GetIdle());
        else
            yield return new WaitForSeconds(GetIdle() / 2);

        //Check shield
        if (shield.isRecharging)
        {
            int recharge = shield.DecreaseRecharge();
            if (recharge <= 0)          
                yield return ActivateShield();
        }

        //Use Desperation attack after X amount of moves and/or hits
        IncrementDespCount();

        StartCoroutine(NextCommand());
    }

    //Moves once then shoots at the player x times
    public IEnumerator JustShoot()
    {
        yield return move.MoveToRandomNode();
        for (int i = 0; i < 3; i++)
        {
            yield return action.ShootSine();
        }
    }

    //Shoots at the player x times. Moves after every shot
    public IEnumerator ShootAndMove()
    {
        yield return action.ShootSine();
        for (int i = 0; i < 2; i++)
        {           
            yield return move.MoveToRandomNode();
            yield return action.ShootSine();
        }
    }

    //Moves once then spin shoots at the player x times
    public IEnumerator JustSpinShoot()
    {
        yield return move.MoveToRandomNode();
        yield return SpinShoot(justSpinShootAmount);
        yield return new WaitForSeconds(actionPause * 3);
    }

    //Spin shoots and moves x times
    public IEnumerator SpinShootAndMove()
    {
        yield return SpinShoot(spinShootAndMoveAmount);
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(actionPause * 3);
            yield return move.MoveToRandomNode();

            yield return SpinShoot(spinShootAndMoveAmount);
        }
        yield return new WaitForSeconds(actionPause * 3);
    }

    //Does a spin shoot x times
    private IEnumerator SpinShoot(int times)
    {
        for (int i = 0; i < times; i++)
        {
            yield return action.ShootSpin();
            yield return new WaitForSeconds(actionPause);
        }
    }


    //Moves once then spin shoots at the player x times
    public IEnumerator DesperationAttack()
    {
        action.ShowDesperationFilter(true);
        yield return move.MoveToNodeCO(0);
        yield return ActivateShield();
        controller.bossAnimator.SetTrigger("Desperation");
        yield return new WaitForSeconds(desperationPause);

        int rng = Random.Range(0, 3);

        for (int i = 0; i < 5; i++)
        {
            if (rng < 2)
                yield return action.ShootDesp1();
            else
                yield return action.ShootDesp2();

            rng = Random.Range(0, 3);
            yield return new WaitForSeconds(desperationPause);
        }
        action.ShowDesperationFilter(false);
        ResetDespCount();
    }

    public void IncrementDespCount()
    {
        if (!despCounterActive)
            return;

        despCounter++;

        if (despCounter >= maxDespCount)
        {
            commandQueue.Enqueue("DesperationAttack");
            
            despCounterActive = false;
        }
    }

    public void ResetDespCount()
    {
        despCounter = 0;
        despCounterActive = true;
    }

    //Moves once then shoots at the player x times
    public IEnumerator ActivateShield()
    {
        controller.bossAnimator.SetTrigger("Get_Shield");

        if(!shield.isShieldActive)
            shield.ShieldActive(true);

        yield return new WaitForSeconds(actionPause * 2);
        ResetAnimation();
    }

    public void ChangeCommandList(int phase)
    {
        commandList.Clear();
        switch (phase)
        {
            case 0: 
                action.doubleProjectiles = false;
                commandList.Add("JustShoot");
                break;

            case 1:
                action.doubleProjectiles = false;
                shield.useShield = true;               

                //commandList.Add("JustShoot");
                commandList.Add("ShootAndMove");

                break;

            case 2:
                action.doubleProjectiles = true;
                shield.useShield = true;

                commandList.Add("JustShoot");
                commandList.Add("JustSpinShoot");

                break;

            case 3:
                action.doubleProjectiles = true;
                shield.useShield = true;

                commandList.Add("ShootAndMove");
                commandList.Add("SpinShootAndMove");

                justSpinShootAmount += 2;
                actionPause -= 0.1f;

                break;

            case 4:
                action.doubleProjectiles = true;
                shield.useShield = true;

                commandQueue.Enqueue("DesperationAttack");

                commandList.Add("ShootAndMove");
                commandList.Add("JustSpinShoot");

                break;

            case 5:
                action.doubleProjectiles = true;
                shield.useShield = true;

                commandList.Add("JustShoot");
                //commandList.Add("ShootAndMove");
                commandList.Add("JustSpinShoot");
                commandList.Add("SpinShootAndMove");

                //actionPause -= 0.1f;
                justSpinShootAmount += 2;
                spinShootAndMoveAmount++;
                action.despShotAmount++;

                break;

            case 6:
                action.doubleProjectiles = true;
                shield.useShield = true;

                commandList.Add("ShootAndMove");
                commandList.Add("JustSpinShoot");
                commandList.Add("SpinShootAndMove");

                commandQueue.Enqueue("DesperationAttack");

                action.despShotAmount++;

                break;

            default:
                Debug.LogError("Error when setting command list. No commands set for phase " + phase);
                commandList.Add("JustShoot");
                break;
        }
    }

    private int NextCommandNumber()
    {
        int result = nextCommandNumber;
        result++;

        if (randomCommands)
        {
            result = Random.Range(0, commandList.Count);

            //Failsafe incase random commands is used with only one command
            if(commandList.Count == 1)
                return 0;

            //Ensure a command isn't used twice in a row
            while (result == nextCommandNumber)           
                result = Random.Range(0, commandList.Count);

            return result;
        }

        if (result >= commandList.Count)
            return 0;

        return result;
    }

    private float GetIdle()
    {
        return idleTime / controller.bossLevel;
    }

    private void ResetAnimation()
    {
        controller.bossAnimator.ResetTrigger("Idle");
        controller.bossAnimator.ResetTrigger("Shoot_Single");
        controller.bossAnimator.ResetTrigger("Shoot_Double");
        controller.bossAnimator.ResetTrigger("Shoot_Spin");
        controller.bossAnimator.ResetTrigger("Desperation");
        controller.bossAnimator.ResetTrigger("Get_Shield");

        controller.bossAnimator.SetTrigger("Idle");
    }
}
