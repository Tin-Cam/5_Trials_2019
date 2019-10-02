using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject player;
    public MainDoor door;
    public List<Grid> roomList = new List<Grid>();

    
    private Grid currentRoom;

    private void Start()
    {
        //LoadRoom(bossList[0], roomList[0]);
    }

    void LoadRoom(_BossBase boss, Grid room)
    {
        door.Open(false);
        LoadBoss(boss);
    }

    private void LoadBoss(_BossBase boss)
    {

    }

    private void UnloadRoom()
    {
        
    }

    public void BossDied()
    {
        door.Open(true);
    }
}
