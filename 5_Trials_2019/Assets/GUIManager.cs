using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public HealthBar bossHealthBar;

    public ScreenFader fader;

    public GameObject gameOverMenu;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowGUI(bool isVisible)
    {
        playerHealthBar.gameObject.SetActive(isVisible);
        bossHealthBar.gameObject.SetActive(isVisible);
    }

    public void ShowGUI_Animate(bool isVisible)
    {
        ShowGUI(isVisible);
    }

    public void InitHealth(float health)
    {
        playerHealthBar.initHealth(health);
    }

    public void AddHealth(float value)
    {
        playerHealthBar.addOrSubtractHealth(value);
    }

    public IEnumerator FadeTransition(string direction)
    {
        if (direction == "In")
            yield return fader.FadeIn();
        else if (direction == "Out")
            yield return fader.FadeOut();
        else
            throw new System.Exception("Error: " + direction + " is not a defined direction");
    }
}
