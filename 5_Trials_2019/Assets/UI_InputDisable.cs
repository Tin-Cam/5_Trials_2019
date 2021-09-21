using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputDisable : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UIEnable(){
        SetUIDisable(false);
    }

    public void UIDisable(){
        SetUIDisable(true);
    }

    private void SetUIDisable(bool value){
        canvasGroup.interactable = !value;
        canvasGroup.blocksRaycasts = !value;
    }
}
