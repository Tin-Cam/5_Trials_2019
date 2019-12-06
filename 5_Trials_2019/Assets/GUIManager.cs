using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public HealthBar bossHealthBar;

    public ScreenFader fader;

    public GameObject gameOverMenu;
    public GameObject pauseMenu;

    private Animator animator;

    void Awake()
    {
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public void ShowGUI(bool isVisible)
    {
        playerHealthBar.gameObject.SetActive(isVisible);
        bossHealthBar.gameObject.SetActive(isVisible);
       
    }

    public void ShowGUI_Animate(bool isVisible)
    {
        animator.SetTrigger("Intro");
        ShowGUI(isVisible);
    }

    public void ShowPause(bool state)
    {
        pauseMenu.SetActive(state);
        fader.FadeMid(state);
    }

    public void ShowGameOver(bool state)
    {
        gameOverMenu.SetActive(state);
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


    public void LoadScene()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
