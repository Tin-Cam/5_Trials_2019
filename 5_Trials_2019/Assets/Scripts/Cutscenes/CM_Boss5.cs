using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CM_Boss5 : MonoBehaviour, ICutsceneManager
{
    public Camera camera;
    private CameraFollow cameraFollow;

    public Collider2D roomEntry;

    public PlayableDirector introCutscene;
    public PlayableDirector endingCutscene;

    public bool playCutscene;

    public GameObject boss;
    public GameObject player;
    public GameObject roomFrontDoor;
    public GameObject cutSceneAssets;
    public Animator light2D;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playCutscene = !FlagManager.instance.boss5Cutscene;
        gameManager = FindObjectOfType<GameManager>();
        camera.transform.position = new Vector3(0,player.transform.position.y, camera.transform.position.z);

        if(!playCutscene)
            SkipIntro();
    }

    public void PlayCutscene(){
        Intro();
    }

    private void Intro(){
        camera.GetComponent<CameraFollow>().enabled = false;       
        introCutscene.stopped += StartFight;
        introCutscene.Play();
    }

    private void SkipIntro(){
        //Initialise room, then start fight;
        //Not gonna lie... Lots of bodged code here lol
        camera.GetComponent<CameraFollow>().enabled = false;
        camera.transform.position = new Vector3(0, 0, camera.transform.position.z);
        player.transform.position = new Vector2(0, -3);
        roomFrontDoor.SetActive(true);
        StartFight();
    }

    //Runs after intro cutscene
    private void StartFight(PlayableDirector cutscene){
        cutscene.stopped -= StartFight;
        StartFight();
    }

    private void StartFight(){
        //Start fight with boss
        boss.SetActive(true);
        Destroy(cutSceneAssets);
        light2D.Play("Light");
        FlagManager.instance.boss6Cutscene = true;
        gameManager.RoomIntro();       
    }

    public void Ending(){

    }
}
