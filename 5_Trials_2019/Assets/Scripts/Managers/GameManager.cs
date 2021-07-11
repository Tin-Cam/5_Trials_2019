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
    private MusicManager musicManager;

    public bool isPaused = false;
    public bool noInterupts = false;

    public GameObject player;
    
    public float playerHealth;
    private float playerMaxHealth;

    private Room room;

    private int currentRoomCode;

    public bool introOnStart = true;

    public AudioSource fanfare;
    public GameObject ambiance;    

    void Awake()
    {
        audioManager = AudioManager.instance;
        roomManager = RoomManager.instance;

        bossManager = GetComponent<BossManager>();
        musicManager = GetComponent<MusicManager>();

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
        musicManager.PlayMusic();
    }

    private IEnumerator RoomIntroCO(){
        Time.timeScale = 0;
        noInterupts = true;

        yield return new WaitForSecondsRealtime(1);

        gui.ShowGUI_Animate(true);
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        noInterupts = false;
        musicManager.PlayMusic();
    }

    public void BossDefeated()
    {
        gui.ShowGUI_Animate(false);
        musicManager.StopMusic();

        //If score mode, go to score screen

        //Enable god mode on player incase they get hit AFTER the boss dies
        player.GetComponentInChildren<PlayerCollision>().godMode = true;

        if(GetBoss().tag == "Boss_5"){
            PrepareCutscene();
            CM_Boss5 cm = FindObjectOfType<CM_Boss5>();
            cm.Ending();
        }
        else if(GetBoss().tag == "Boss_6"){
            PrepareCutscene();
            CM_Boss6 cm = FindObjectOfType<CM_Boss6>();
            cm.Ending();
        }
        else
            StartCoroutine(BossDefeatedCO());
    } 

    private IEnumerator BossDefeatedCO(){
        yield return new WaitForSeconds(2);
        fanfare.Play();
        //yield return new WaitWhile (() => audioManager.GetSound("Fanfare_Short").source.isPlaying);
        yield return new WaitForSeconds(3);
        OpenRoomDoor();
        ambiance.SetActive(true);
    }

    private void PrepareCutscene(){
        player.GetComponentInChildren<PlayerCollision>().godMode = true;
        noInterupts = true;
        DeleteObjectsOfTag("Projectile");
    }

    public void OpenRoomDoor()
    {
        //Play victory music
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
        musicManager.StopMusic();
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
