using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class levelSelect : MonoBehaviour
{
    public List<GameObject> levels = new List<GameObject>();
    public TextMeshProUGUI difficultyText;

    private MenuManager menuManager;
    private int selectedDifficulty = 0;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();

        foreach(GameObject level in levels)
            level.SetActive(false);

        int progress = FlagManager.instance.GetGameProgress();
        if(progress > levels.Count)
            progress = levels.Count;

        for(int i = 0; i <= progress; i++)
            levels[i].SetActive(true);
    }

    public void StartLevel(int level){
        menuManager.StartGame(selectedDifficulty, level);
    }

    public void SetSelectedDifficulty(int value){
        selectedDifficulty = value;

        string text = "";
        if(selectedDifficulty == 0)
            text = "Easy Mode";
        else
            text = "Normal Mode";
        difficultyText.text = text;
    }
}
