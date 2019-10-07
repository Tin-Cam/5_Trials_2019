using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject player;
    public MainDoor door;
    public List<Room> roomList = new List<Room>();

    
    private Room currentRoom;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    //Make a corutine?
    public Room LoadRoom(int roomCode)
    {
        UnloadRoom();
        

        try
        {
            currentRoom = CreateRoom(roomCode);
        }
        catch (MissingReferenceException)
        {
            currentRoom = Instantiate(roomList[0]);
            Debug.Log("No room to load");
        }
        catch (System.ArgumentException)
        {
            currentRoom = Instantiate(roomList[0]);
            Debug.Log("No room to load");
        }

        return currentRoom;
    }

    private Room CreateRoom(int roomCode)
    {
        Room room;

        room = Instantiate(roomList[roomCode]);
        room.SetGameManager(gameManager);

        return room;
    }

    private void UnloadRoom()
    {
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

    }

    public void OpenRoomDoor()
    {
        currentRoom.OpenDoor();
    }
}
