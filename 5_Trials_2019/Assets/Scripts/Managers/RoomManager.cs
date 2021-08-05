using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;
    
    private int currentRoom = 9;

    private ScreenFader fader;

    private bool isLoading = false;

    private List<string> scenes = new List<string> {
        "Main_Menu",    //0
        "Starting_Room",//1
        "Boss1",  //2
        "Boss2",   //3
        "Boss3",   //4
        "Boss4",   //5
        "Boss5",   //6
        "Boss6",   //7
        "Cutscene_Interlude", //8
        "Ending" //9
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
        StartCoroutine(FindScreenFaderCO());
    }

    private IEnumerator FindScreenFaderCO(){
        while(fader == null){
            fader = FindObjectOfType<ScreenFader>();
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator OnRoomLoad(){
        if(SceneManager.GetActiveScene().buildIndex == currentRoom)
            yield break;

        while (SceneManager.GetActiveScene().buildIndex != currentRoom)
            yield return new WaitForSecondsRealtime(0.1f);
        
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
        //LoadRoom(scenes[GetRoomCode()]);

        SceneManager.LoadScene(scenes[GetRoomCode()]);
        StartCoroutine(OnRoomLoad());
    }

    public void ResetToFirstBoss(){
        //LoadRoom("Boss1");

        SceneManager.LoadScene("Boss1");
        StartCoroutine(OnRoomLoad());
    }

    public void LoadRoom(string room){
        int roomCode = scenes.IndexOf(room);
        LoadRoom(roomCode);
    }

    public void LoadRoom(int roomCode){
        if(!isLoading){
            isLoading = true;
            StartCoroutine(LoadRoomCO(roomCode));
        }
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
        isLoading = false;
    }

    public void LoadInterludeCutscene(int cutsceneCode){
        if(!isLoading){
            isLoading = true;
            StartCoroutine(LoadInterludeCutsceneCO(cutsceneCode));
        }
    }

    private IEnumerator LoadInterludeCutsceneCO(int cutsceneCode){
        yield return LoadRoomCO(scenes.IndexOf("Cutscene_Interlude"));

        Interlude interlude = FindObjectOfType<Interlude>();
        interlude.InterludeEffects(cutsceneCode);
        yield return new WaitForSeconds(1);
        interlude.LoadCutscene(cutsceneCode);
        Time.timeScale = 1;
    }
}