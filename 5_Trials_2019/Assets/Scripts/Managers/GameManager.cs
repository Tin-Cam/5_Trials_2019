using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private RoomManager roomManager;
    //private BossManager bossManager;

    public int startingRoom;
    public GameObject player;
    public float playerMaxHealth;
    private float playerHealth;

    public HealthBar playerHealthBar;
    public bool hardcore;

    [Space(15)]
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

        LoadNewRoom(startingRoom);
        
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

    public _BossBase GetBoss()
    {
        return roomManager.GetBoss();
    }

    public void PlayerTakeDamage(float damage)
    {
        playerHealth -= damage;
        playerHealthBar.addOrSubtractHealth(-damage);
        if (playerHealth <= 0)
            player.SetActive(false);
    }
}
