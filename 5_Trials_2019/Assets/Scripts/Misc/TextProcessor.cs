using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class TextProcessor
{
    public AudioSource textSound;

    private float textSpeed;
    private float defaultTextSpeed = 1f;
    private int textSoundOccurance = 4;

    private bool speedUpText;
    [HideInInspector]
    public bool writingText;

    private string[] textLines;
    private IEnumerator writingCO;
    private TextMeshProUGUI textBox;

    private int nextLine;

    private int lastTInt;

    public TextProcessor(TextMeshProUGUI textBox){
        textSpeed = defaultTextSpeed;
        this.textBox = textBox;
        writingText = false;
    }

    public TextProcessor(TextMeshProUGUI textBox, float speed){
        defaultTextSpeed = speed;
        textSpeed = defaultTextSpeed;
        this.textBox = textBox;
        writingText = false;
    }

    public List<string> TextFileParser(string textFile){
        List<string> result = textFile.Split('|').ToList();
        return result;
    }

    public List<string> LineParser(string line){
        List<string> result = line.Split('{').ToList();
        return result;
    }

    public IEnumerator WriteText(string textToWrite){
        float t = 0;
        speedUpText = false;
        writingText = true;
        textBox.text = textToWrite;
        //Write the characters
        while(t < textToWrite.Length){
            t += Time.deltaTime * textSpeed;
            int tInt = Mathf.RoundToInt(t);
            //Check to ensure t doesn't exceed string
            if(tInt >= textToWrite.Length)
                tInt = textToWrite.Length - 1;
           
            textSpeed = CheckCharacter(textToWrite[tInt]);
            
            textBox.maxVisibleCharacters = tInt + 1;
            TextSound(tInt);

            yield return null;
        }
        textBox.text = textToWrite;
        writingText = false;
    }

    //Plays sound when the amount of characters on screen is divisible by a set value
    public void TextSound(int amount){
        if(amount == lastTInt || speedUpText)
            return;

        if(amount % textSoundOccurance == 0){
            try{
                    textSound.PlayOneShot(textSound.clip, 0.7f);
                }
            catch(System.NullReferenceException){
                    Debug.LogWarning("No audio source has been set for text");
            }
        }
        lastTInt = amount;
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
}


