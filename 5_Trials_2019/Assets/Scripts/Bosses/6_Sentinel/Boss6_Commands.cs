using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Commands : MonoBehaviour
{
    private Boss6_Action action;
    private Boss6_Move move;
    private Boss6_Controller controller;


    public int circleSpread;
    public float circleOffset;

    public bool randomCommands;
    private List<string> commandList = new List<string>();
    private Queue<string> commandQueue = new Queue<string>();
    private int nextCommandNumber;

    public void Init()
    {
        action = GetComponent<Boss6_Action>();
        move = GetComponent<Boss6_Move>();
        controller = GetComponent<Boss6_Controller>();

        ChangeCommandList(0);

        StartCoroutine(NextCommand());
    }

    public IEnumerator NextCommand()
    {
        //ResetAnimation();

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

        //ResetAnimation();

        StartCoroutine(NextCommand());
    }

    //Used for testing
    public IEnumerator MrTest()
    {
        yield return CircleAndShoot();
              
    }

    public IEnumerator SpinShoot()
    {
        yield return move.Teleport(Vector3.zero);
        yield return action.SpinShoot();
    }

    public IEnumerator SweepShoot()
    {
        int times = 20;
        int lastNode = 0;

        for (int i = 0; i < times; i++)
        {
            int rng = Random.Range(1, 5);
            while (rng == lastNode)
                rng = Random.Range(1, 5);
            lastNode = rng;

            Vector3 node = move.InnerNodes[rng].position;
            yield return move.Teleport(node);
            yield return action.SweepShoot((-90) * (rng - 1));
        }
    }

    public IEnumerator CircleAndShoot()
    {
        //Starts the movement
        yield return move.ZipToPosition(move.CirclePos(Vector2.zero, 5f, 0));
        StartCoroutine(move.CircleCentre());

        //Starts shooting
        for (int i = 0; i < 20; i++)
        {
            action.ShootAtPlayer(circleSpread, circleOffset);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1);

        //Exits the action
        move.isCircling = false;
        yield return move.Exit();
    }


    public IEnumerator Teleport()
    {
        yield return move.RandomTeleport();
    }

    public IEnumerator GlideOver()
    {
        //Set start position
        float rng = Random.Range(-3, 3);
        Vector2 point = new Vector2(-12, rng);
        transform.position = point;

        //Set and move to end position
        rng = Random.Range(-3, 3);
        point = new Vector2(12, rng);
        yield return move.GlideToPosition(point);
    }

    public void ChangeCommandList(int phase)
    {
        commandList.Clear();
        switch (phase)
        {
            case 0:
                commandList.Add("MrTest");
                break;

            case 1:

                break;

            default:
                Debug.LogError("Error when setting command list. No commands set for phase " + phase);
                commandList.Add("MrTest");
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
            if (commandList.Count == 1)
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