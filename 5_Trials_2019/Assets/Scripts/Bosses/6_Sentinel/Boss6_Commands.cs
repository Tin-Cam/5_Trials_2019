﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Commands : MonoBehaviour
{
    private Boss6_Action action;
    private Boss6_Move move;
    private Boss6_Controller controller;

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
        yield return move.RandomTeleport();
        //yield return new WaitForSeconds(1);
    }

    public IEnumerator CircleAndShoot()
    {
        //Starts the movement
        yield return move.MoveToPosition(move.CirclePos(Vector2.zero, 5f, 0));
        StartCoroutine(move.CircleCentre());

        //Starts shooting
        for (int i = 0; i < 20; i++)
        {
            yield return action.ShootAtPlayer();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1);

        //Exits the action
        move.isCircling = false;
        yield return move.Exit();
        yield break;
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