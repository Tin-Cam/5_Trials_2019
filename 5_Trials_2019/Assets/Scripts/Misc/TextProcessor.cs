using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class TextProcessor
{
    private float textSpeed;
    private float defaultTextSpeed = 1f;
    private bool speedUpText;

    public TextProcessor(){
        textSpeed = defaultTextSpeed;
    }

    public TextProcessor(float speed){
        defaultTextSpeed = speed;
        textSpeed = defaultTextSpeed;
    }

    public List<string> TextFileParser(string textFile){
        List<string> result = textFile.Split('|').ToList();
        return result;
    }

    public List<string> LineParser(string line){
        List<string> result = line.Split('{').ToList();
        return result;
    }

    public IEnumerator WriteSection(string textToWrite, TextMeshProUGUI textBox){
        int length = textToWrite.Length;
        
        for(int i = 0; i < textToWrite.Length; i++){
            textBox.text = textToWrite.Substring(0, i);
            yield return CheckCharacter(textToWrite[i]);
        }
    }

    public IEnumerator WriteText(string textToWrite, TextMeshProUGUI textBox){
        float t = 0;
        speedUpText = false;
        while(t < textToWrite.Length){
            int tInt = (int) t;

            textBox.text = WriteCharacters(textToWrite, tInt);
            textSpeed = CheckCharacter(textToWrite[tInt]);
            t += Time.deltaTime * textSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

    public string WriteCharacters(string text, int characters){
        string result = text.Substring(0, characters);
        return result;
    }

    public float CheckCharacter(char character){
        if(speedUpText)
            return defaultTextSpeed * 100;

        float result = defaultTextSpeed;
        switch(character){
            case '.':
                result = defaultTextSpeed * 0.5f;
                break; 
            default:
                break;
        }
        return result;
    }

    public void ProgressText(){
        //Speed up text if not complete
        speedUpText = true;

    }

    private class TextData {
        public List<string> lines;
    }
}


