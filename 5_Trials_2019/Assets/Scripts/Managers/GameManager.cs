using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    private RoomManager roomManager;
    public GUIManager gui;
    //private BossManager bossManager;

    public bool isPaused = false;
    public bool noInterupts = false;

    public int startingRoom;
    public GameObject player;
    
    public float playerHealth;
    private float playerMaxHealth;

    [Space(15)]
    public _BossBase testBoss;
    //public HealthBar bossHealthBar;

    private int currentRoomCode;

    void Awake()
    {
        roomManager = GetComponent<RoomManager>();
        
        audioManager = FindObjectOfType<AudioManager>();

        playerMaxHealth = playerHealth;
        gui.InitHealth(playerMaxHealth);      
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseGame(!isPaused);
        }
    }

    public void LoadNewRoom(int roomCode)
    {
        bool transition = true;

        if (roomCode == 0)
            transition = false;
        StopAllCoroutines();
        StartCoroutine(LoadRoomCo(roomCode, transition));
    }

    private IEnumerator LoadRoomCo(int roomCode, bool transition)
    {
        
        currentRoomCode = roomCode;

        //Swap Rooms
        noInterupts = true;
        Time.timeScale = 0;

        if (transition)
            yield return gui.FadeTransition("Out");

        DeleteObjectsOfTag("Projectile");
        roomManager.LoadRoom(roomCode);

        //Restores Player health if not on hardcore mode
        if (GameData.difficulty != 2)
            ResetHealth();

        bool hasBoss = roomManager.RoomHasBoss();
        gui.ShowGUI(false);
        
        if (hasBoss)
        {
            //Plays the boss intro
            yield return gui.FadeTransition("In");

            yield return new WaitForSecondsRealtime(1);

            gui.ShowGUI_Animate(hasBoss);
            yield return new WaitForSecondsRealtime(1);
        }
        else
        {
            yield return gui.FadeTransition("In");                               
        }
        Time.timeScale = 1;
        noInterupts = false;
    }

    public void BossDefeated()
    {
        gui.ShowGUI_Animate(false);
        OpenRoomDoor();
        //Stop Music
    }   

    public void OpenRoomDoor()
    {
        roomManager.OpenRoomDoor();
    }

    public void PauseGame(bool state)
    {
        if (noInterupts)
            return;

        isPaused = state;
        gui.ShowPause(isPaused);
        audioManager.Play("Button_Press");

        if (!isPaused)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void ResetRoom()
    {
        
        ResetHealth();
        player.SetActive(true);
        player.GetComponent<PlayerAttack>().DefaultState();

        gui.ShowGameOver(false);
        StartCoroutine(LoadRoomCo(currentRoomCode, false));
    }

    private void ResetHealth()
    {
        playerHealth = playerMaxHealth;
        gui.InitHealth(playerMaxHealth);
    }

    private void DeleteObjectsOfTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject item in objects)
        {
            Destroy(item);
        }
    }

    public _BossBase GetBoss()
    {
        return roomManager.GetBossBase();
    }

    public void PlayerTakeDamage(float damage)
    {
        audioManager.Play("Player_Hit");
        playerHealth -= damage;
        gui.AddHealth(-damage);
        if (playerHealth <= 0)
            StartCoroutine(PlayerDeath());
    }

    private IEnumerator PlayerDeath()
    {
        audioManager.Play("Player_Death");
        player.SetActive(false);
        yield return new WaitForSeconds(1);
        yield return gui.FadeTransition("Out");
        Time.timeScale = 0;
        gui.ShowGameOver(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
