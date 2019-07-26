using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float playerMaxHealth;
    private float playerHealth;

    public HealthBar healthBar;

    void Start()
    {
        healthBar.setHealth(playerMaxHealth);
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            healthBar.addHealth(-1);
        }
    }
}
