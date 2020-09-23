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

    public List<TextAsset> cutscenes = new List<TextAsset>();

    private TextProcessor textProcessor;

    private int currentLine = 0;

    private Cutscene currentCutscene;
    
    // Start is called before the first frame update
    void Start(){
        //LoadCutscene(3);
    }

    public void LoadCutscene(int cutsceneID)
    {
        textProcessor = new TextProcessor(textSpeed);
        string jsonFile = cutscenes[cutsceneID].text;
        currentCutscene = JsonUtility.FromJson<Cutscene>(jsonFile);

        NextLine();
    }

    void Update() {
        if (Input.GetButtonDown("Attack"))        
            NextLine();  
    }

    private void NextLine(){
        if(!textProcessor.IsTextComplete())
            return;
        if(currentLine >= currentCutscene.text.Length)
            return;
        StartCoroutine(textProcessor.WriteText(currentCutscene.text[currentLine], textBox));
        currentLine++;
    }

    private class Cutscene {
        public string name;
        public string[] text;
        public string nextScene;
    }
}
