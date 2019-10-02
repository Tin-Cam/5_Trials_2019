﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool debugMode;

    private GameManager gameManager;
    private _BossBase boss;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        boss = gameManager.GetBoss();
    }

    // Update is called once per frame
    void Update()
    {
        if (!debugMode)
            return;
        BossCommands();
    }

    private void BossCommands()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            boss.PickAction(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            boss.PickAction(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            boss.PickAction(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            boss.PickAction(3);
        }

        //Stops the boss from acting
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            boss.StopAction();
        }
    }
}
