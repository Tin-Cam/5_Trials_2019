﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Commands : MonoBehaviour
{
    private Boss6_Action action;
    private Boss6_Move move;
    private Boss6_Controller controller;

    public bool randomCommands;
    public int idleTime;

    public int sweepShootTimes;

    public int circleSpread;
    public float circleOffset;

    public int gridGlides;


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
        //StartCoroutine(action.SineAttack());
    }

    public IEnumerator NextCommand()
    {
        DefaultState();
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

    public IEnumerator Idle()
    {
        float time = idleTime * GetDelay();
        yield return new WaitForSeconds(time);
    }

    //Used for testing
    public IEnumerator MrTest()
    {
        yield return DesperationAttack();
    }

    public IEnumerator SweepShoot()
    {
        int times = (int)(sweepShootTimes * controller.bossLevel);       

        int rng = Random.Range(1, 5);
        int lastNode = rng;
        yield return move.EnterToInner(rng);
        yield return action.SweepShoot((-90) * (rng - 1));

        for (int i = 0; i < (times - 1); i++)
        {           
            while (rng == lastNode)
                rng = Random.Range(1, 5);
            lastNode = rng;

            Vector3 node = move.InnerNodes[rng].position;
            yield return move.Teleport(node);
            yield return action.SweepShoot((-90) * (rng - 1));
        }
        yield return Idle();
        yield return move.Exit();
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

    public IEnumerator SpinShoot()
    {
        yield return move.Teleport(Vector3.zero);
        yield return action.SpinShoot();
        yield return Idle();
        yield return move.Teleport(new Vector3(0, 20, 0));
    }

    public IEnumerator TargetPlayer()
    {
        yield return move.EnterToInner(0);
        yield return new WaitForSeconds(0.2f);
        yield return action.AimAtPlayer();
        yield return action.TargetPlayer();
        yield return move.Exit();
    }

    public IEnumerator TargetPlayerAndAttack()
    {
        yield return move.EnterToInner(0);
        yield return new WaitForSeconds(0.2f);
        yield return action.AimAtPlayer();
        StartCoroutine(action.TargetPlayerHold());
        yield return move.Exit();

        int rng = Random.Range(0, 3);
        switch(rng){
            case 0:
                yield return SweepShoot();
                break;
            case 1:
                yield return CircleAndShoot();
                break;
            case 2:
                yield return SpinShoot();
                break;
            default:
                yield return SweepShoot();
                break;
        }
        action.holdTargeting = false; 
        yield return Idle();
    }

    public IEnumerator TargetPlayerInfinite()
    {
        yield return move.EnterToInner(0);
        yield return new WaitForSeconds(0.2f);
        yield return action.AimAtPlayer();
        StartCoroutine(action.TargetPlayerHold());
        yield return move.Exit();
        yield return Idle();
    }

    public IEnumerator GridAttack()
    {
        //Play animation when gliding

        //Start Attack
        yield return GlideOver();
        StartCoroutine(action.GridAttack());
        while(!action.holdGrid)
            yield return new WaitForFixedUpdate();
        
        //Glide over stage X times
        int times = (int)(gridGlides * controller.bossLevel);
        for(int i = 0; i < times; i++)
            yield return GlideOver();

        action.holdGrid = false;
        yield return Idle();
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

    public IEnumerator DesperationAttack()
    {
        yield return move.MoveToDesperation();
        yield return new WaitForSeconds(1 * GetDelay());
        yield return action.SineAttack();
        yield return Idle();
        yield return move.Exit();
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

    private float GetDelay()
    {
        return 1 / controller.bossLevel;
    }

    public void DefaultState()
    {
        action.DefaultState();
    }
}