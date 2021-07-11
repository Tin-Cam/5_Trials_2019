using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class Interlude : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public TextAsset text;
    public float textSpeed;
    public GameObject textIcon;

    public List<TextAsset> cutscenes = new List<TextAsset>();

    public MusicManager music;

    private TextProcessor textProcessor;

    private int nextLine = 0;

    private Cutscene currentCutscene;
    
    // Start is called before the first frame update
    void Start(){
        //LoadCutscene(3);
    }

    public void LoadCutscene(int cutsceneID)
    {
        textProcessor = new TextProcessor(textBox, textSpeed);
        string jsonFile = cutscenes[cutsceneID].text;
        currentCutscene = JsonUtility.FromJson<Cutscene>(jsonFile);
        nextLine = 0;

        InterludeEffects(cutsceneID);

        music.PlayMusic();
        NextLine();
    }

    //Changes the background and music pitch depending on which cutscene is playing
    private void InterludeEffects(int cutsceneID){
        if(cutsceneID == 3){
            music.source.pitch = 0.8f;
        }
        else if(cutsceneID == 4){
            music.source.pitch = 0.6f;
        }
        else if(cutsceneID == 5){
            music.source.pitch = 0.2f;
        }
    }

    void Update() {
        if (Input.GetButtonDown("Attack"))        
            NextLine();
    }

    private void NextLine(){
        //Check if the next line can be written
        if(!textProcessor.IsTextComplete())
            return;
        if(nextLine >= currentCutscene.text.Length){
                RoomManager.instance.LoadRoom(currentCutscene.nextScene);
                return;
        }
        //Write next line
        StartCoroutine(NextLineCO());
        nextLine++;
    }

    //Cuts the music when the last line of the last interlude is shown (for drama)
    private void LastInterludeLastLineCheck(){

    }

    private IEnumerator NextLineCO(){
        SetTextIconActive(false);
        yield return textProcessor.WriteText(currentCutscene.text[nextLine]);
        SetTextIconActive(true);
    }

    private void SetTextIconActive(bool isActive){
        if(!isActive){
            textIcon.SetActive(false);
            return;
        }
        //Display different icons depending on what line is displayed
        textIcon.SetActive(true);
    }

    private class Cutscene {
        public string name;
        public string[] text;
        public string nextScene;
    }
}
