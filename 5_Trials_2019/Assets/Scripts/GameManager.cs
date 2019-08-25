using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        if (Input.GetButtonDown("Attack"))
        {  
            //healthBar.addOrSubtractHealth(-1);
        }

    }

    void updateBoss()
    {
        //boss = newBoss
        boss.healthBar = bossHealthBar;
    }

    public void playerTakeDamage(float damage)
    {
        playerHealth -= damage;
        playerHealthBar.addOrSubtractHealth(-1);
        if (playerHealth <= 0)
            player.SetActive(false);
    }
}
