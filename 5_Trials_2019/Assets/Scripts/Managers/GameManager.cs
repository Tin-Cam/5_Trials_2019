using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;
    [HideInInspector]
    public RoomManager roomManager;

    public GUIManager gui;
    private BossManager bossManager;

    public bool isPaused = false;
    public bool noInterupts = false;

    public GameObject player;
    
    public float playerHealth;
    private float playerMaxHealth;

    private Room room;

    private int currentRoomCode;

    public bool introOnStart = true;

    void Awake()
    {
        audioManager = AudioManager.instance;
        roomManager = RoomManager.instance;

        bossManager = GetComponent<BossManager>();

        gui.ShowGUI(false);
    }

    void Start(){
        room = FindObjectOfType<Room>();

        playerMaxHealth = playerHealth;
        gui.InitHealth(playerMaxHealth);

        if(introOnStart)
            RoomIntro();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseGame(!isPaused);
        }
    }

    public void RoomIntro(){
        StartCoroutine(RoomIntroCO());
    }

    public void QuickRoomIntro(){
        gui.ShowGUI_Animate(true);
    }

    private IEnumerator RoomIntroCO(){
        Time.timeScale = 0;
        noInterupts = true;

        yield return new WaitForSecondsRealtime(1);

        gui.ShowGUI_Animate(true);
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        noInterupts = false;
    }

    public void BossDefeated()
    {
        gui.ShowGUI_Animate(false);
        //Stop Music

        if(bossManager.currentBoss.tag == "Boss_5"){
            
        }
        else if(bossManager.currentBoss.tag == "Boss_6"){

        }
        else
            OpenRoomDoor();        
    }   

    public void OpenRoomDoor()
    {
        room.OpenDoor();
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

    public void ResetRoom(){
        roomManager.ReloadRoom();
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
        return bossManager.currentBoss;
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
        DeleteObjectsOfTag("Projectile");
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
