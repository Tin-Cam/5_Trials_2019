using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject player;
    public MainDoor door;
    public List<_BossBase> bossList = new List<_BossBase>();
    public List<Grid> roomList = new List<Grid>();

    
    private _BossBase currentBoss;
    private Grid currentRoom;

    private void Start()
    {
        LoadRoom(bossList[0], roomList[0]);
    }

    void LoadRoom(_BossBase boss, Grid room)
    {
        door.Open(false);
        LoadBoss(boss);
    }

    private void LoadBoss(_BossBase boss)
    {
        currentBoss = boss;
        currentBoss.roomManager = this;
        Instantiate(currentBoss, transform);
    }

    private void UnloadRoom()
    {
        
    }

    public void BossDied()
    {
        door.Open(true);
    }
}
