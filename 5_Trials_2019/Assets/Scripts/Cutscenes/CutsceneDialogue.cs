using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class CutsceneDialogue : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public TextAsset textSource;
    public float textSpeed;
    public GameObject textIcon;

    public List<Sprite> portaits = new List<Sprite>();

    private Dialogue currentDialogue;

    private TextProcessor textProcessor;

    private int nextLine = 0;

    
    // Start is called before the first frame update
    void Start(){
        LoadDialogue();
        Debug.Log(currentDialogue.lines[0].text);
        Debug.Log(currentDialogue.lines[1].text);
        Debug.Log(currentDialogue.lines[2].text);
    }

    public void LoadDialogue()
    {
        textProcessor = new TextProcessor(textBox, textSpeed);
        string jsonFile = JsonUtility.ToJson(textSource.text);
        Debug.Log(jsonFile);
        currentDialogue = JsonUtility.FromJson<Dialogue>(jsonFile);
        nextLine = 0;

        NextLine();
    }

    void Update() {
        if (Input.GetButtonDown("Attack"))        
            NextLine();  
    }

    private void NextLine(){
        //Check if the next line can be written
        if(!textProcessor.IsTextComplete())
            return;
        //Write next line
        StartCoroutine(NextLineCO());
        nextLine++;
    }

    private IEnumerator NextLineCO(){
        SetTextIconActive(false);
        //yield return textProcessor.WriteText(currentDialogue.lines[nextLine].text);
        SetTextIconActive(true);
        yield return null;
    }

    private void SetTextIconActive(bool isActive){
        if(!isActive){
            textIcon.SetActive(false);
            return;
        }
        //Display different icons depending on what line is displayed
        textIcon.SetActive(true);
    }

    private class Dialogue {
        //public string name;
        public Line[] lines;
    }

    private class Line {
        //public string name;
        public string text;
        //public string name2;
        public int portrait;
    }
}

