using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Commands : MonoBehaviour
{
    public float idleTime;

    public float actionPause;
    public float desperationPause;

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

        if(commandQueue.Count != 0)
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
        
        yield return new WaitForSeconds(idleTime);

        //Check shield
        if (shield.isRecharging)
        {
            int recharge = shield.DecreaseRecharge();
            if(recharge <= 0)
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
            yield return new WaitForSeconds(actionPause);
        }
    }

    //Shoots at the player x times. Moves after every shot
    public IEnumerator ShootAndMove()
    {
        yield return action.ShootSine();
        for (int i = 0; i < 2; i++)
        {           
            yield return move.MoveToRandomNode();
            yield return new WaitForSeconds(actionPause);
            yield return action.ShootSine();
        }
    }

    //Moves once then spin shoots at the player x times
    public IEnumerator JustSpinShoot()
    {
        yield return move.MoveToRandomNode();
        for (int i = 0; i < 3; i++)
        {
            yield return action.ShootSpin();
            yield return new WaitForSeconds(actionPause);
        }
    }

    //Spin shoots and moves x times
    public IEnumerator SpinShootAndMove()
    {
        yield return action.ShootSpin();
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(actionPause * 3);
            yield return move.MoveToRandomNode();
            yield return action.ShootSpin();           
        }
    }


    //Moves once then spin shoots at the player x times
    public IEnumerator DesperationAttack()
    {
        action.ShowDesperationFilter(true);
        yield return move.MoveToNodeCO(0);
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
        shield.ShieldActive(true);
        yield return new WaitForSeconds(actionPause); 
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
                action.doubleProjectiles = true;
                shield.useShield = true;

                //commandQueue.Enqueue("DesperationAttack");

                commandList.Add("JustShoot");
                commandList.Add("ShootAndMove");

                break;

            case 2:
                action.doubleProjectiles = true;
                shield.useShield = true;

                commandQueue.Enqueue("DesperationAttack");

                commandList.Add("JustShoot");
                commandList.Add("ShootAndMove");
                commandList.Add("SpinShootAndMove");

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
}
