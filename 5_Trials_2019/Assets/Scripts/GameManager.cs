using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float playerMaxHealth;
    private float playerHealth;

    public HealthBar healthBar;

    public _BossBase boss;
    public HealthBar bossHealthBar;

    void Start()
    {
        //healthBar.setHealth(playerMaxHealth);
        
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
}
