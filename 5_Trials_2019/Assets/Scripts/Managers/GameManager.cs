using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private RoomManager roomManager;
    public GUIManager gui;
    //private BossManager bossManager;

    public bool isPaused;
    public int startingRoom;
    public GameObject player;
    
    public float playerHealth;
    private float playerMaxHealth;

    //public HealthBar playerHealthBar;
    public bool hardcore;

    [Space(15)]
    public _BossBase testBoss;
    //public HealthBar bossHealthBar;

    private int currentRoomCode;

    void Awake()
    {
        roomManager = GetComponent<RoomManager>();
        //bossManager = GetComponent<BossManager>();

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
        currentRoomCode = roomCode;
        StartCoroutine(gui.FadeTransition("In"));
        roomManager.LoadRoom(roomCode);
        gui.ShowGUI_Animate(roomManager.RoomHasBoss());
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
        isPaused = state;
        gui.ShowPause(isPaused);

        if (!isPaused)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void ResetRoom()
    {
        LoadNewRoom(currentRoomCode);
        DeleteObjectsOfTag("Projectile");

        playerHealth = playerMaxHealth;
        gui.InitHealth(playerMaxHealth);
        player.SetActive(true);
        player.GetComponent<PlayerAttack>().DefaultState();

        gui.ShowGameOver(false);
        Time.timeScale = 1;
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
        playerHealth -= damage;
        gui.AddHealth(-damage);
        if (playerHealth <= 0)
            StartCoroutine(PlayerDeath());
    }

    private IEnumerator PlayerDeath()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(1);
        yield return gui.FadeTransition("Out");
        Time.timeScale = 0;
        gui.ShowGameOver(true);
    }
}
