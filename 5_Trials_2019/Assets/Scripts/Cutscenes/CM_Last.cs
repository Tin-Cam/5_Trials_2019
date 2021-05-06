using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CM_Last : MonoBehaviour
{
    public PlayableDirector endingCutscene;

    public CutsceneDialogue dialogue;

    public ScreenFader fader;

    public GameObject theEndText;

    private GameManager gameManager;

    void Awake(){
        fader.e_FadeIn.AddListener(PlayCutscene);
        endingCutscene.stopped += ShowDialogue;
    }

    // Start is called before the first frame update
    void Start()
    {
        endingCutscene.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    private void PlayCutscene(){
        fader.e_FadeIn.RemoveListener(PlayCutscene);
        endingCutscene.playableGraph.GetRootPlayable(0).SetSpeed(1);
        endingCutscene.Play();
    }

    private void ShowDialogue(PlayableDirector cutscene){
        endingCutscene.stopped -= ShowDialogue;
        dialogue.gameObject.SetActive(true);
        dialogue.e_Finished.AddListener(Finish);
        dialogue.LoadDialogue();
    }

    private void Finish(){
        dialogue.e_Finished.RemoveListener(Finish);
        dialogue.gameObject.SetActive(false);
        StartCoroutine(FinishCO());
    }

    private IEnumerator FinishCO(){
        yield return new WaitForSeconds(2);
        fader.fadeAnimationSpeed += 0.2f;
        yield return fader.FadeOut();
        theEndText.SetActive(true);
        yield return new WaitForSeconds(6);
        RoomManager.instance.LoadRoom(0);
    }
}
