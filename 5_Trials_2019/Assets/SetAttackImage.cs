using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAttackImage : MonoBehaviour
{
    //Changes the object's sprite if the Mac flag is true
    //(Mainly used to make Mac porting quicker)
    
    public Sprite macSprite;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        if(FlagManager.instance.macBuild){
            image.sprite = macSprite;
        }
    }
}
