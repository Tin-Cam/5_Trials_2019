using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameManager gameManager;

    public Vector2 playerSpawn;
    public int bossCode; //Set to 0 if there is no boss in the room
    public AudioClip music;
    public MainDoor door;


    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void OpenDoor()
    {
        door.Open(true);
    }
}
