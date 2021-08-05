using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CM_Last : MonoBehaviour
{
    public PlayableDirector endingCutscene;

    public CutsceneDialogue dialogue;

    public TextAsset endingStandard;
    public TextAsset endingFlawless;
    public TextAsset endingNoHit;
    public TextAsset endingCheat;

    public ScreenFader fader;

    public GameObject theEndText;

    public AudioSource music;

    private GameManager gameManager;

    public GameObject skipCutscenePrompt;

    private int cutsceneSkipCounter = 3;
    private float skipTimer = 0;
    private bool playing;

    void Awake(){
        fader.e_FadeIn.AddListener(PlayCutscene);
        endingCutscene.stopped += ShowDialogue;
    }

    // Start is called before the first frame update
    void Start()
    {
        endingCutscene.playableGraph.GetRootPlayable(0).SetSpeed(0);
        playing = true;
    }

    void Update(){
        if (playing && Input.GetButtonDown("Attack"))
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
            playing = false;
            endingCutscene.playableGraph.GetRootPlayable(0).SetSpeed(0);
            endingCutscene.stopped -= ShowDialogue;
            Finish();
            return;
        }
        cutsceneSkipCounter--;
        skipCutscenePrompt.SetActive(true);
    }

    private void PlayCutscene(){
        fader.e_FadeIn.RemoveListener(PlayCutscene);
        endingCutscene.playableGraph.GetRootPlayable(0).SetSpeed(1);
        endingCutscene.Play();
    }

    private void ShowDialogue(PlayableDirector cutscene){
        endingCutscene.stopped -= ShowDialogue;
        playing = false;
        skipTimer = 0;
        music.Play();

        dialogue.textSource = GetEndingDialogueText();
        dialogue.gameObject.SetActive(true);
        dialogue.e_Finished.AddListener(Finish);
        dialogue.LoadDialogue();
    }

    //Changes the ending dailogue depending on how the game was played
    private TextAsset GetEndingDialogueText(){
        FlagManager flagManager = FlagManager.instance;
        CheatMode cheatMode = FindObjectOfType<CheatMode>();

        if(cheatMode.isActivated)
            return endingCheat;

        if(!flagManager.hasBeenHit && !flagManager.easyMode)
            return endingNoHit;

        if(flagManager.flawlessMode)
            return endingFlawless;

        return endingStandard;
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
        yield return new WaitForSeconds(12);
        RoomManager.instance.LoadRoom(0);
    }
}
