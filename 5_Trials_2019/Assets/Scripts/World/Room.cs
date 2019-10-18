using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameManager gameManager;

    public bool cameraLockX;
    public bool cameraLockY;

    public bool hasIntro;
    public Vector2 playerSpawn;
    public int bossID; //Set to 0 if there is no boss in the room
    public AudioClip music = null;
    public MainDoor door = null;

    public void ChangeRoom(int destination)
    {
        gameManager.LoadNewRoom(destination);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public GameManager GetGameManager()
    {
        return gameManager;
    }

    public void OpenDoor()
    {
        door.Open(true);
    }
}
