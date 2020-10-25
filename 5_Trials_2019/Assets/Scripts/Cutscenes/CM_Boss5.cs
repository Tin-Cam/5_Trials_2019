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
    public GameObject cutSceneAssets;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playCutscene = !FlagManager.instance.boss5Cutscene;
        gameManager = FindObjectOfType<GameManager>();

        //if(playCutscene)
        //    Intro();
        //else
        //    SkipIntro();
    }

    public void PlayCutscene(){
        Intro();
    }

    private void Intro(){
        camera.GetComponent<CameraFollow>().enabled = false;
        introCutscene.Play();
    }

    private void SkipIntro(){
        //Initialise room, then start fight;
        StartFight();
    }

    private void StartFight(){
        //Start fight with boss
    }

    public void Ending(){

    }
}
