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
        "Cutscene_Interlude", //9
        "Ending" //10
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

    void Start(){
        currentRoom = GetRoomCode();
        StartCoroutine(OnRoomLoad());
    }

    private IEnumerator OnRoomLoad(){
        if(SceneManager.GetActiveScene().buildIndex == currentRoom)
            yield break;

        while (SceneManager.GetActiveScene().buildIndex != currentRoom)
            yield return null;
        
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
        LoadRoom(scenes[GetRoomCode()]);
    }   

    public void LoadRoom(string room){
        int roomCode = scenes.IndexOf(room);
        LoadRoom(roomCode);
    }

    public void LoadRoom(int roomCode){
        StartCoroutine(LoadRoomCO(roomCode));
    }

    private IEnumerator LoadRoomCO(int roomCode){
        currentRoom = roomCode;

        if(fader != null){
            Time.timeScale = 0;
            yield return fader.FadeOut();
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(scenes[roomCode]);
        yield return OnRoomLoad();
    }

    public void LoadInterludeCutscene(int cutsceneCode){
        StartCoroutine(LoadInterludeCutsceneCO(cutsceneCode));
    }

    private IEnumerator LoadInterludeCutsceneCO(int cutsceneCode){
        yield return LoadRoomCO(scenes.IndexOf("Cutscene_Interlude"));

        Interlude interlude = FindObjectOfType<Interlude>();
        interlude.LoadCutscene(cutsceneCode);
        Time.timeScale = 1;
    }
}