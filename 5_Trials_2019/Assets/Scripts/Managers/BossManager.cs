using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private GameManager gameManager;

    public _BossHolder currentBoss;

    public GameObject player;
    public HealthBar healthBar;


    [Space(15)]
    public List<_BossHolder> bossList = new List<_BossHolder>();

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void LoadBoss(int bossID)
    {
        if (IsBossListEmpty())
            return;

        if (bossID == 0)
            return; 

        UnloadBoss();
        currentBoss = Instantiate(CreateBoss(bossID));
    }

    private bool IsBossListEmpty()
    {
        if (bossList.Capacity > 0)
            return false;

        return true;
    }


    public _BossHolder CreateBoss(int bossID)
    {
        _BossHolder boss;

        boss = bossList[bossID];

        boss.SetVariables(player, healthBar, gameManager);

        return boss;
    }

    //Unloads the boss if the player leaves the room
    //(Theoretically should not trigger in normal gameplay)
    public void UnloadBoss()
    {
        try
        {
            Destroy(currentBoss.gameObject);
            currentBoss = null;
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("No boss to unload");
        }
        catch (MissingReferenceException)
        {
            Debug.Log("No boss to unload");
        }
    }

    public _BossHolder GetBoss()
    {
        return currentBoss;
    }

    public _BossBase GetBossBase()
    {
        return currentBoss.boss;
    }

    //REDUNDANT
    public void SetVariables(GameObject player, HealthBar healthBar)
    {
        this.player = player;
        this.healthBar = healthBar;
    }
}
