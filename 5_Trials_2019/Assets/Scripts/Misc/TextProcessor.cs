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
    private bool writingText = false;

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
        writingText = true;
        while(t < textToWrite.Length){
            t += Time.deltaTime * textSpeed;
            int tInt = Mathf.RoundToInt(t);
            if(tInt >= textToWrite.Length)
                tInt = textToWrite.Length - 1;
           
            textSpeed = CheckCharacter(textToWrite[tInt]);            
            textBox.text = textToWrite.Substring(0, tInt);
            yield return new WaitForFixedUpdate();
        }
        textBox.text = textToWrite;
        writingText = false;
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

    public bool IsTextComplete(){
        //Speed up text if not complete
        if(writingText){
            speedUpText = true;
            return false;;
        }
        return true;
    }

    private class TextData {
        public List<string> lines;
    }
}


