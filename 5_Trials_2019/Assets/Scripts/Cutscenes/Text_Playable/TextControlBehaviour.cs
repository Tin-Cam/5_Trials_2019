using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

[Serializable]
public class TextControlBehaviour : PlayableBehaviour
{
    [SerializeField]
    private string text = "Default Text";

    [SerializeField]
    private float textSpeed = 1;

    private bool firstFrameHappened;
    private string defaultText;

    private TextMeshPro textBox;
    private float textAmount = 0;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        textBox = playerData as TextMeshPro;

        if(textBox == null)
            return;

        if(!firstFrameHappened){
            defaultText = textBox.text;

            firstFrameHappened = true;
        }

        textBox.text = CalculateText(playable);
    }

    private string CalculateText(Playable playable){
        int characters = 0;
        
        characters = Mathf.RoundToInt(textSpeed * (float)playable.GetTime());

        string result = text.Substring(0, characters);

        return result;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        firstFrameHappened = false;

        if(textBox == null)
            return;

        textBox.text = defaultText;

        base.OnBehaviourPause(playable, info);
    }

    
}
