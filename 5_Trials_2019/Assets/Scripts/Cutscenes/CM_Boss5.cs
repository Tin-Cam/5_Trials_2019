using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class CM_Boss5 : MonoBehaviour, ICutsceneManager
{
    public Camera camera;
    private CameraFollow cameraFollow;

    public Collider2D roomEntry;

    public PlayableDirector introCutscene;
    public PlayableDirector endingCutscene;

    public bool playCutscene;
    private bool playingEnding;

    public GameObject boss;
    public GameObject player;
    public GameObject roomFrontDoor;
    public GameObject cutSceneAssets;
    public Animator light2D;
    public AudioSource fanfare;
    public GameObject skipCutscenePrompt;

    private int cutsceneSkipCounter = 3;
    private float skipTimer = 0;

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

    void Update(){
        if (playingEnding && Input.GetButtonDown("Attack"))
        {
            skipTimer = 3;
            Debug.Log("Skip int: " + cutsceneSkipCounter);
            CutsceneSkipper();
        }
        if(skipTimer > 0){
            skipTimer -= Time.deltaTime;
        }
        else{
            skipCutscenePrompt.SetActive(false);
            cutsceneSkipCounter = 3;
        }
    }
    private void CutsceneSkipper(){
        if(cutsceneSkipCounter <= 0){
            playingEnding = false;
            endingCutscene.playableGraph.GetRootPlayable(0).SetSpeed(0);
            NextRoom(endingCutscene);
            return;
        }
        cutsceneSkipCounter--;
        skipCutscenePrompt.SetActive(true);
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
        light2D.Play("2D_Light");
        FlagManager.instance.boss5Cutscene = true;
        gameManager.RoomIntro();       
    }

    public void Ending(){
        StartCoroutine(EndingCO());
    }

    public IEnumerator EndingCO(){
        yield return new WaitForSeconds(2);
        fanfare.Play();
        yield return new WaitWhile (() => fanfare.isPlaying);
        //yield return new WaitForSeconds(2);
        playingEnding = true;
        endingCutscene.stopped += NextRoom;
        endingCutscene.Play();
    }

    //Runs after intro cutscene
    private void NextRoom(PlayableDirector cutscene){
        cutscene.stopped -= NextRoom;
        RoomManager.instance.LoadInterludeCutscene(5);
    }
}
