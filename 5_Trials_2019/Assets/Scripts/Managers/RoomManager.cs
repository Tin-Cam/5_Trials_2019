using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private GameManager gameManager;
    private BossManager bossManager;

    public CameraFollow cameraFollow;

    public GameObject player;
    public MainDoor door;
    public List<Room> roomList = new List<Room>();
    public int startingRoom;
    
    private Room currentRoom;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        bossManager = GetComponent<BossManager>();
    }

    void Start()
    {
        LoadRoom(startingRoom);
    }

    //Make a corutine?
    public void LoadRoom(int roomCode)
    {
        UnloadRoom();
        
        try
        {
            currentRoom = CreateRoom(roomCode);
            bossManager.LoadBoss(currentRoom.bossID);
            player.transform.position = currentRoom.playerSpawn;
            SetCamera();
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
        
    }

    //Create a room from a prefab
    private Room CreateRoom(int roomCode)
    {
        Room room;
        
        room = Instantiate(roomList[roomCode]);
        room.SetGameManager(gameManager);
        
        return room;
    }

    private void SetCamera()
    {     
        cameraFollow.lockX = currentRoom.cameraLockX;
        cameraFollow.lockY = currentRoom.cameraLockY;

        if (cameraFollow.lockX & cameraFollow.lockY)
        {
            cameraFollow.ResetCamera();
            return;
        }

        cameraFollow.ResetCamera(player.transform.position);
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

        bossManager.UnloadBoss();
    }


    public void OpenRoomDoor()
    {
        currentRoom.OpenDoor();
    }

    public bool RoomHasBoss()
    {
        if (currentRoom.bossID == 0)
            return false;

        return true;
    }

    public _BossBase GetBossBase()
    {
        return bossManager.GetBossBase();
    }
}