using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DifficultyButton : MonoBehaviour, ISelectHandler
{
    public DifficultyDescriptions descriptions;
    public int descriptionID;

    public void OnSelect(BaseEventData eventData)
    {
        descriptions.SetDescription(descriptionID);
    }
}
