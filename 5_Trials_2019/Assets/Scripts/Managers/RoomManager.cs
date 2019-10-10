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

    private void Start()
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

    private Room CreateRoom(int roomCode)
    {
        Room room;
        

        room = Instantiate(roomList[roomCode]);
        room.SetGameManager(gameManager);

        //Creates a boss in the room if it has one
        if (room.bossCode != 0)
        {
            _BossBase boss;
            boss = bossManager.CreateBoss(room.bossCode);
            currentBoss = Instantiate(boss);
            //currentBoss.gameObject.transform.SetParent(currentRoom.gameObject.transform);
        }

        
        return room;
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
}
