using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private RoomManager roomManager;
    //private BossManager bossManager;

    public bool isPaused;
    public int startingRoom;
    public GameObject player;
    public float playerMaxHealth;
    private float playerHealth;

    public HealthBar playerHealthBar;
    public bool hardcore;

    [Space(15)]
    public _BossBase testBoss;
    public HealthBar bossHealthBar;

    [Space(15)]
    public ScreenFader fader;

    void Start()
    {
        roomManager = GetComponent<RoomManager>();
        //bossManager = GetComponent<BossManager>();

        playerHealth = playerMaxHealth;
        playerHealthBar.initHealth(playerMaxHealth);      
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
        fader.FadeIn();
        roomManager.LoadRoom(roomCode);
        ShowGUI_Animate(roomManager.RoomHasBoss());
    }

    public void BossDefeated()
    {
        ShowGUI_Animate(false);
        OpenRoomDoor();
        //Stop Music
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

    public void OpenRoomDoor()
    {
        roomManager.OpenRoomDoor();
    }

    public void PauseGame(bool state)
    {
        isPaused = state;

        if (state)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    

    public _BossBase GetBoss()
    {
        return roomManager.GetBossBase();
    }

    public void PlayerTakeDamage(float damage)
    {
        playerHealth -= damage;
        playerHealthBar.addOrSubtractHealth(-damage);
        if (playerHealth <= 0)
            player.SetActive(false);
    }
}
