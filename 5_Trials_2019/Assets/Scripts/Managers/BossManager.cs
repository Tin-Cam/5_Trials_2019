using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject player;
    public HealthBar healthBar;

    [Space(15)]
    public List<_BossBase> bosses = new List<_BossBase>();

    
    public _BossBase CreateBoss(int bossID)
    {
        _BossBase boss = bosses[bossID];

        boss.player = player;
        boss.healthBar = healthBar;

        return boss;
    }

    //REDUNDANT
    public void SetVariables(GameObject player, HealthBar healthBar)
    {
        this.player = player;
        this.healthBar = healthBar;
    }
}
