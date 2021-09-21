using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Portrait : MonoBehaviour


{
    public List<Image> images = new List<Image>();
    public int unlockValue;
    public string name;
    public TextMeshProUGUI nameText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateVisible();
    }

    public void UpdateVisible(){
        int progress = FlagManager.instance.GetGameProgress();
        if(progress >= unlockValue){
            SetVisible(true);
            UncensorName(true);
        }
        else {
            SetVisible(false);
            UncensorName(false);
        }
    }

    private void SetVisible(bool isVisible){
        Color color;

        if(isVisible)
            color = Color.white;
        else
            color = Color.black;

        foreach(Image image in images)
            image.color = color;
    }

    private void UncensorName(bool value){
        string newName = "??????";
        if(value)
            newName = name;
        nameText.text = newName;
    }

}
