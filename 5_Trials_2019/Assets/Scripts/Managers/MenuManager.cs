using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject cursor;
    private GameObject selectedItem;
    private EventSystem eventSystem;

    public List<GameObject> menus = new List<GameObject>();

    private GameObject currentMenu;

    
    void Start()
    {
        eventSystem = EventSystem.current;

        //The first menu in the list is loaded first
        currentMenu = menus[0];
        currentMenu.SetActive(true);

        for (int i = 1; i < menus.Capacity; i++)
            menus[i].SetActive(false);

        SelectTopItem();
    }

    void Update() {
        if(HasCursorChanged()){
            UpdateCursor();
            PlaySelectSound();
        }
    }

    private void SelectTopItem(){
        Menu menu = currentMenu.GetComponent<Menu>();
        selectedItem = menu.topButton;
        eventSystem.SetSelectedGameObject(selectedItem);
        UpdateCursor();
    }

    private bool HasCursorChanged(){
        //Checks if the selected UI item has changed 
        if(eventSystem.currentSelectedGameObject == selectedItem)
            return false;

        //Checks to see if nothing is selected (And then sets it to the previous selected item)
        if(eventSystem.currentSelectedGameObject == null){
            eventSystem.SetSelectedGameObject(selectedItem);
            return false;
        }

        return true;
    }


    private void UpdateCursor(){   
        selectedItem = eventSystem.currentSelectedGameObject;

        Vector2 newPos = new Vector2(cursor.transform.position.x, selectedItem.transform.position.y);
        cursor.transform.position = newPos;       
    }


    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void StartGame(int difficulty)
    {
        PlayButtonSound();

        if(difficulty == 0)
            FlagManager.instance.easyMode = true;
        else
            FlagManager.instance.easyMode = false;

        FlagManager.instance.bossDeaths = 0;
        
        //LoadScene("Starting_Room");
        RoomManager.instance.LoadInterludeCutscene(0);
    }

    public void QuitGame()
    {
        PlayButtonSound();
        Application.Quit();
    }

    public void ChangeMenu(int menu)
    {
        PlayButtonSound();
        currentMenu.SetActive(false);
        currentMenu = menus[menu];
        currentMenu.SetActive(true);
        SelectTopItem();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.Play("Button_Press");
    }

    public void PlaySelectSound()
    {
        AudioManager.instance.Play("Button_Select");
    }

    public void SetSelectedItem(GameObject item){
        eventSystem.SetSelectedGameObject(item);
        UpdateCursor();
        PlaySelectSound();
    }
}
