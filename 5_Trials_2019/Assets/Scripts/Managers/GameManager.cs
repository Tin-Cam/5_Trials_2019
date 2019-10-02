using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RoomManager roomManager;

    public GameObject player;
    public float playerMaxHealth;
    private float playerHealth;

    public HealthBar playerHealthBar;

    public _BossBase boss;
    public HealthBar bossHealthBar;


    void Start()
    {
        playerHealth = playerMaxHealth;
        playerHealthBar.initHealth(playerMaxHealth);
        
    }

    void Update()
    {

    }

    void updateBoss()
    {
        //boss = newBoss
        boss.healthBar = bossHealthBar;
    }

    public _BossBase GetBoss()
    {
        return boss;
    }

    public void playerTakeDamage(float damage)
    {
        playerHealth -= damage;
        playerHealthBar.addOrSubtractHealth(-1);
        if (playerHealth <= 0)
            player.SetActive(false);
    }
}
