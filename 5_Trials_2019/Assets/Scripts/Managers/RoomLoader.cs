using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    public static RoomLoader instance;
    
    private int currentRoom = 9;

    private List<string> scenes = new List<string> {
        "Main_Menu",    //0
        "Rooms",        //1
        "Demo_End",     //2
        "SampleScene",  //3
        "Boss2_Test",   //4
        "Boss3_Test",   //5
        "Boss4_Test",   //6
        "Boss5_Test",   //7
        "Boss6_Test",   //8
        "RoomLoader_Test1",   //9
        "RoomLoader_Test2",   //10
    };

    void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
    }

    public void MoveRooms(int direction) {
        currentRoom += direction;
        if(currentRoom < 0)
            currentRoom += scenes.Count;
        if(currentRoom >= scenes.Count)
            currentRoom -= scenes.Count;
        LoadRoom(currentRoom);
    }

    public int GetRoomCode(){
        return currentRoom;
    }

    public void ReloadRoom(){
        SceneManager.LoadScene(scenes[currentRoom]);
    }

    public void LoadRoom(int roomCode){
        currentRoom = roomCode;
        Debug.Log("Loading room " + roomCode + ": " + scenes[roomCode]);
        SceneManager.LoadScene(scenes[roomCode]);
    }

    public void LoadInbetweenCutscene(int cutsceneCode){

    }
}