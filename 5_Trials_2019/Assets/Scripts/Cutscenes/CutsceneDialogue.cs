using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Events;

public class CutsceneDialogue : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public TextAsset textSource;
    public float textSpeed;
    public GameObject textIcon;

    public Image portrait;
    public List<Sprite> portaits = new List<Sprite>();

    private Dialogue currentDialogue;

    private TextProcessor textProcessor;

    private int currentLine = 0;

    public UnityEvent e_Finished;

    
    // Start is called before the first frame update
    void Start(){
        //LoadDialogue();
    }

    public void LoadDialogue()
    {
        textProcessor = new TextProcessor(textBox, textSpeed);
        string jsonFile = textSource.text;
        Debug.Log(jsonFile);
        currentDialogue = JsonUtility.FromJson<Dialogue>(jsonFile);
        currentLine = 0;

        WriteLine();
    }

    void Update() {
        if (Input.GetButtonDown("Attack"))        
            WriteLine();  
    }

    private void WriteLine(){
        //Check if the next line can be written
        if(!textProcessor.IsTextComplete())
            return;
        //Check if there are anymore lines to write
        if(currentLine >= currentDialogue.lines.Length){
            EndDialogue();
            return;
        }
        //Change portrait image
        int nextPortrait = currentDialogue.lines[currentLine].portrait;
        portrait.sprite = portaits[nextPortrait];        
        //Write next line
        StartCoroutine(NextLineCO());
        currentLine++;
    }

    private IEnumerator NextLineCO(){    
        SetTextIconActive(false);
        yield return textProcessor.WriteText(currentDialogue.lines[currentLine].text);
        SetTextIconActive(true);

    }

    private void EndDialogue(){
        e_Finished.Invoke();
    }

    private void SetTextIconActive(bool isActive){
        if(!isActive){
            textIcon.SetActive(false);
            return;
        }
        //Display different icons depending on what line is displayed
        textIcon.SetActive(true);
    }

    [Serializable]
    private class Dialogue {
        public Line[] lines;
    }

    [Serializable]
    private class Line {
        public string text;
        public int portrait;
    }
}

