using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject player;
    public HealthBar healthBar;

    [Space(15)]
    public List<_BossHolder> bosses = new List<_BossHolder>();

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public _BossHolder CreateBoss(int bossID)
    {
        _BossHolder boss = bosses[bossID];

        boss.SetVariables(player, healthBar, gameManager);

        return boss;
    }

    //REDUNDANT
    public void SetVariables(GameObject player, HealthBar healthBar)
    {
        this.player = player;
        this.healthBar = healthBar;
    }
}
