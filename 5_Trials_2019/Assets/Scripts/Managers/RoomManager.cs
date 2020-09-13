using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    
    private int currentRoom = 9;

    private ScreenFader fader;

    private List<string> scenes = new List<string> {
        "Main_Menu",    //0
        "Starting_Room",//1
        "Demo_End",     //2
        "Boss1",  //3
        "Boss2",   //4
        "Boss3",   //5
        "Boss4",   //6
        "Boss5",   //7
        "Boss6",   //8
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
        OnRoomLoad();
    }

    private void OnRoomLoad(){
        fader = FindObjectOfType<ScreenFader>();
        if(fader != null)
            StartCoroutine(fader.FadeIn());       
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
        string scene = SceneManager.GetActiveScene().name;
        Debug.Log("Scene: " + scene);
        int result = scenes.IndexOf(scene);
        return result;
    }

    public void ReloadRoom(){
        SceneManager.LoadScene(scenes[GetRoomCode()]);
    }

    public void LoadRoom(int roomCode){
        currentRoom = roomCode;
        Debug.Log("Loading room " + roomCode + ": " + scenes[roomCode]);
        SceneManager.LoadScene(scenes[roomCode]);
    }

    public void LoadRoom(string room){
        int roomCode = scenes.IndexOf(room);
        Debug.Log("Loading room " + roomCode + ": " + scenes[roomCode]);
        StartCoroutine(LoadRoomCO(roomCode));
    }

    private IEnumerator LoadRoomCO(int roomCode){
        if(fader != null)
            yield return fader.FadeOut();
        SceneManager.LoadScene(scenes[roomCode]);
        OnRoomLoad();
    }

    public void LoadInbetweenCutscene(int cutsceneCode){

    }
}