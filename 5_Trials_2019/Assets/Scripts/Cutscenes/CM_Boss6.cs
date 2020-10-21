using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CM_Boss6 : MonoBehaviour
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
        gameManager = FindObjectOfType<GameManager>();
        if(playCutscene)
            Intro();
        else
            SkipIntro();
    }

    private void Intro(){
        //introCutscene.Play();
        //StartFight();
    }

    private void SkipIntro(){
        //Initialise room, then start fight;
        cutSceneAssets.SetActive(false);
        boss.SetActive(true);
        StartFight();
    }

    private void StartFight(){
        //Start fight with boss
        gameManager.RoomIntro();
    }

    public void Ending(){

    }
}
