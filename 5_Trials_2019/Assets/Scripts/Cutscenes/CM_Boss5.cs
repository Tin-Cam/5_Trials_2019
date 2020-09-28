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

    // Start is called before the first frame update
    void Start()
    {
        //Check if cutscene has already been played

    }

    public void PlayCutscene(){
        Intro();
    }

    private void Intro(){
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
