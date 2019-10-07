using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameManager gameManager;

    public bool hasMovingCamera;
    public bool hasIntro;
    public Vector2 playerSpawn;
    public int bossCode; //Set to 0 if there is no boss in the room
    public AudioClip music;
    public MainDoor door;

    public List<RoomExit> exits = new List<RoomExit>();

    private void Start()
    {
        for (int i = 0; i < exits.Count; i++)
            exits[i].SetGameManager(gameManager);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void OpenDoor()
    {
        door.Open(true);
    }

    //Sets active of all exits
    public void EnableExit(bool isActive)
    {
        for (int i = 0; i < exits.Count; i++)
            exits[i].gameObject.SetActive(isActive);
    }

    //Sets active of given exit
    public void EnableExit(bool isActive, int exit)
    {
        exits[exit].gameObject.SetActive(isActive);
    }
}
