using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject player;
    public HealthBar healthBar;

    [Space(15)]
    public List<_BossBase> bosses = new List<_BossBase>();

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public _BossBase CreateBoss(int bossID)
    {
        _BossBase boss = bosses[bossID];

        boss.player = player;
        boss.healthBar = healthBar;
        boss.SetGameManager(gameManager);

        return boss;
    }

    //REDUNDANT
    public void SetVariables(GameObject player, HealthBar healthBar)
    {
        this.player = player;
        this.healthBar = healthBar;
    }
}
