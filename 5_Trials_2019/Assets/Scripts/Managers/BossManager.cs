using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private GameManager gameManager;

    public _BossBase currentBoss;

    public GameObject player;
    public HealthBar healthBar;


    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        currentBoss = FindObjectOfType<_BossBase>();
    }
    
}