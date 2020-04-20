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
            PerformCommand(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PerformCommand(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PerformCommand(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            PerformCommand(3);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            PerformCommand(4);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            PerformCommand(5);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            PerformCommand(6);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            PerformCommand(7);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            PerformCommand(8);
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            PerformCommand(9);
        }

        //Stops the boss from acting
        if (Input.GetKeyDown(KeyCode.KeypadPeriod))
        {
            GetBoss().StopAction();
        }
    }

    private void PerformCommand(int action)
    {
        GetBoss().StopAction();
        try {
            GetBoss().PickAction(action);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Debug.Log("No command set to this key");
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
