using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyDescriptions : MonoBehaviour
{
    public List<GameObject> descriptions = new List<GameObject>();

    public void SetDescription(int id){
        foreach(GameObject text in descriptions){
            text.SetActive(false);
        }
        descriptions[id].SetActive(true);
    }
}
