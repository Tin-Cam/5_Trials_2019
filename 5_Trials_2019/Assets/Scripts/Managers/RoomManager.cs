using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private GameManager gameManager;
    private BossManager bossManager;

    public GameObject player;
    public MainDoor door;
    public List<Room> roomList = new List<Room>();
      
    private Room currentRoom;
    private _BossBase currentBoss;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        bossManager = GetComponent<BossManager>();
    }

    //Make a corutine?
    public Room LoadRoom(int roomCode)
    {
        UnloadRoom();
        
        try
        {
            currentRoom = CreateRoom(roomCode);
            currentBoss = CreateBoss();
            player.transform.position = currentRoom.playerSpawn;
        }
        catch (MissingReferenceException)
        {
            LoadRoom(0);
            Debug.Log("No room to load. Room has been set to default.");
        }
        catch (System.ArgumentException)
        {
            LoadRoom(0);
            Debug.Log("No room to load. Room has been set to default.");
        }

        return currentRoom;
    }

    //Create a room from a prefab
    private Room CreateRoom(int roomCode)
    {
        Room room;
        
        room = Instantiate(roomList[roomCode]);
        room.SetGameManager(gameManager);
        
        return room;
    }

    //Creates a boss for the room if it has one
    private _BossBase CreateBoss()
    {
        if (currentRoom.bossCode == 0)
            return null;
        
        _BossBase boss;
        boss = bossManager.CreateBoss(currentRoom.bossCode);
        boss = Instantiate(boss);

        return boss;
    }

    private void UnloadRoom()
    {
        //Destroys current room
        try
        {
            Destroy(currentRoom.gameObject);
            currentRoom = null;
        }
        catch(System.NullReferenceException)
        {
            Debug.Log("No room to unload");
        }
        catch (MissingReferenceException)
        {
            Debug.Log("No room to unload");
        }

        //Destroys current boss
        try
        {
            Destroy(currentBoss.gameObject);
            currentBoss = null;
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("No boss to unload");
        }
        catch (MissingReferenceException)
        {
            Debug.Log("No boss to unload");
        }

    }


    public void OpenRoomDoor()
    {
        currentRoom.OpenDoor();
    }

    public bool RoomHasBoss()
    {
        if (currentRoom.bossCode == 0)
            return false;

        return true;
    }

    public _BossBase GetBoss()
    {
        return currentBoss;
    }
}
