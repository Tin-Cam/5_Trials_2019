using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameManager gameManager;

    public bool cameraLockX;
    public bool cameraLockY;

    public AudioClip music;
    public MainDoor door;

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
