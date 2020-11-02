using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class CM_Boss6 : MonoBehaviour, ICutsceneManager
{
    public PlayableDirector introCutscene;
    public PlayableDirector endingCutscene;

    public bool playCutscene;

    public GameObject boss;
    public GameObject cutSceneAssets;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playCutscene = !FlagManager.instance.boss6Cutscene;
        gameManager = FindObjectOfType<GameManager>();

        if(playCutscene)
            Intro();
        else
            SkipIntro();
    }

    public void PlayCutscene(){
        Intro();
    }

    private void Intro(){
        introCutscene.Play();
        introCutscene.stopped += StartFight;
    }

    private void SkipIntro(){
        //Initialise room, then start fight;
        cutSceneAssets.SetActive(false);
        boss.SetActive(true);
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

        if(playCutscene){
            gameManager.QuickRoomIntro();
            FlagManager.instance.boss6Cutscene = true;
        }
        else
            gameManager.RoomIntro();       
    }

    public void Ending(){

    }

    private IEnumerator WaitForCutscene(PlayableDirector cutscene){
        while(cutscene.state == PlayState.Playing)
            yield break;
    }
}
