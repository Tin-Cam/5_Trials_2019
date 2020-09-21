using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class Inbetween : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public TextAsset text;
    public float textSpeed;

    public List<TextAsset> cutscenes = new List<TextAsset>();

    private TextProcessor textProcessor;
    
    // Start is called before the first frame update
    void Start()
    {
        textProcessor = new TextProcessor(textSpeed);
        string jsonFile = cutscenes[0].text;
        Cutscene cutscene = JsonUtility.FromJson<Cutscene>(jsonFile);

        Debug.Log(jsonFile);
        Debug.Log(cutscene.name);
        Debug.Log(cutscene.text[0]);

        StartCoroutine(textProcessor.WriteText(cutscene.text[0], textBox));
    }

    void Update() {
        if (Input.GetButtonDown("Attack"))
            textProcessor.ProgressText();
    }

    private class Cutscene {
        public string name;
        public string[] text;
    }
}
