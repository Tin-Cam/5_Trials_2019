using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public List<_BossBase> bosses = new List<_BossBase>();

    private GameObject player;
    private HealthBar healthBar;

    public _BossBase CreateBoss(int bossID)
    {
        _BossBase boss = bosses[bossID];

        boss.player = player;
        boss.healthBar = healthBar;

        return boss;
    }

    public void SetVariables(GameObject player, HealthBar healthBar)
    {
        this.player = player;
        this.healthBar = healthBar;
    }
}
