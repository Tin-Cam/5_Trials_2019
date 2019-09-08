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

    [Space(15)]
    public bool debugMode;

    void Start()
    {
        playerHealth = playerMaxHealth;
        playerHealthBar.initHealth(playerMaxHealth);
        
    }

    void Update()
    {
        if (debugMode)
            debug();

    }

    void debug()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            testBossAction(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            testBossAction(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            testBossAction(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            testBossAction(3);
        }
    }

    void testBossAction(int action)
    {
        boss.pickAction(action);
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
