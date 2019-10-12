using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool debugMode;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!debugMode)
            return;
        BossCommands();
        RoomCommands();
    }

    private void BossCommands()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            GetBoss().PickAction(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            GetBoss().PickAction(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            GetBoss().PickAction(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            GetBoss().PickAction(3);
        }

        //Stops the boss from acting
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            GetBoss().StopAction();
        }
    }

    private void RoomCommands()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameManager.LoadNewRoom(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameManager.LoadNewRoom(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameManager.LoadNewRoom(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameManager.LoadNewRoom(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            gameManager.LoadNewRoom(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            gameManager.OpenRoomDoor();
        }
    }

    private _BossBase GetBoss()
    {
        return gameManager.GetBoss();
    }
}
