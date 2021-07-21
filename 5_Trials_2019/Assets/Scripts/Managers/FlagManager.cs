using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager instance;

    public bool easyMode;
    public bool flawlessMode;
    public int flawlessHealth;
    [Space(10)]
    public bool boss5Cutscene;
    public bool boss6Cutscene;
    [Space(10)]
    public bool hasDied;
    public bool hasBeenHit;
    public float bossTime;
    public int bossId;

    [HideInInspector]
    public int bossDeaths;

    void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
    }

    public void SetToDefault(){
        boss5Cutscene = false;
        boss6Cutscene = false;

        bossDeaths = 0;
        hasDied = false;
        hasBeenHit = false;
    }
}
