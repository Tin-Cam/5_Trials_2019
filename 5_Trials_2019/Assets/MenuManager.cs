using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> menus = new List<GameObject>();

    private GameObject currentMenu;

    
    void Start()
    {
        //The first menu in the list is loaded first
        currentMenu = menus[0];
        currentMenu.SetActive(true);

        for (int i = 1; i < menus.Capacity; i++)
            menus[i].SetActive(false);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void StartGame(int difficulty)
    {
        LoadScene("Rooms");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeMenu(int menu)
    {
        currentMenu.SetActive(false);
        currentMenu = menus[menu];
        currentMenu.SetActive(true);
    }
}
