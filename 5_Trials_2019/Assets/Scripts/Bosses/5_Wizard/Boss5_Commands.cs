using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Commands : MonoBehaviour
{
    public float idleTime;

    public float actionPause;

    private Boss5_Action action;
    private Boss5_Move move;
    private Boss5_Controller controller;
    private Boss5_Shield shield;

    public bool randomCommands;
    private List<string> commandList = new List<string>();
    private int nextCommandNumber;

    // Start is called before the first frame update
    public void Init()
    {
        action = GetComponent<Boss5_Action>();
        move = GetComponent<Boss5_Move>();
        controller = GetComponent<Boss5_Controller>();
        shield = GetComponent<Boss5_Shield>();

        ChangeCommandList(1);

        StartCoroutine(NextCommand());
    }

    public IEnumerator NextCommand()
    {
        nextCommandNumber = NextCommandNumber();
        yield return StartCoroutine(commandList[nextCommandNumber]);
        yield return new WaitForSeconds(idleTime);

        //Check shield
        if (shield.isRecharging)
        {
            int recharge = shield.DecreaseRecharge();
            if(recharge <= 0)
                yield return ActivateShield();
        }

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
            case 1:
                commandList.Add("JustShoot");
                commandList.Add("ShootAndMove");
                commandList.Add("JustSpinShoot");
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
            return Random.Range(0, commandList.Count);
        }

        if (result >= commandList.Count)
            return 0;

        return result;
    }

}
