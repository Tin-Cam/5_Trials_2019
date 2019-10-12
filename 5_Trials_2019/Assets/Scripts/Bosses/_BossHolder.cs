using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BossHolder : MonoBehaviour
{
    public _BossBase boss;

    void SetGameManager(GameManager gameManager)
    {
        boss.gameManager = gameManager;
    }

    public void SetVariables(GameObject player, HealthBar healthBar, GameManager gameManager)
    {
        boss.player = player;
        boss.healthBar = healthBar;
        boss.SetGameManager(gameManager);
    }

    public void SetDisabled()
    {

    }

    public _BossBase GetBoss()
    {
        return boss;
    }
}
