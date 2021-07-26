using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<GameObject> stars = new List<GameObject>();

    void Start()
    {
        int currentStars = PlayerPrefs.GetInt("stars", 0);

        for(int i = 0; i < currentStars; i++){
            stars[i].SetActive(true);
        }
        
    }
}
