using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private RoomManager roomManager;
    //private BossManager bossManager;

    public GameObject player;
    public float playerMaxHealth;
    private float playerHealth;

    public HealthBar playerHealthBar;
    public bool hardcore;

    [Space(15)]
    public _BossBase boss;
    public HealthBar bossHealthBar;

    [Space(15)]
    public ScreenFader fader;

    void Start()
    {
        roomManager = GetComponent<RoomManager>();
        //bossManager = GetComponent<BossManager>();

        //bossManager.SetVariables(player, bossHealthBar);

        playerHealth = playerMaxHealth;
        playerHealthBar.initHealth(playerMaxHealth);

        LoadNewRoom(1);
        
    }

    public void LoadNewRoom(int roomCode)
    {
        fader.FadeIn();
        roomManager.LoadRoom(roomCode);
        

    }

    public void OpenRoomDoor()
    {
        roomManager.OpenRoomDoor();
    }

    public _BossBase GetBoss()
    {
        return boss;
    }

    public void PlayerTakeDamage(float damage)
    {
        playerHealth -= damage;
        playerHealthBar.addOrSubtractHealth(-damage);
        if (playerHealth <= 0)
            player.SetActive(false);
    }
}
