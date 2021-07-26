using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager instance;

    public bool easyMode;
    public bool flawlessMode;
    public float flawlessHealth;
    private float maxFlawlessHealth = 5;
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
            flawlessHealth = 5;
		}
    }

    public void ResetFlawlessMode(){
        flawlessHealth = maxFlawlessHealth;
        bossDeaths = 0;
        hasDied = false;
        hasBeenHit = false;
    }

    public void SetToDefault(){
        boss5Cutscene = false;
        boss6Cutscene = false;

        flawlessHealth = maxFlawlessHealth;
        bossDeaths = 0;
        hasDied = false;
        hasBeenHit = false;
    }

    public void SetStars(){
        int starsEarned = CalculateStars();
        int currentStars = PlayerPrefs.GetInt("stars", 0);

        if(starsEarned > currentStars)
            PlayerPrefs.SetInt("stars", starsEarned);
    }

    private int CalculateStars()
    {
        CheatMode cheatMode = GetComponent<CheatMode>();

        //Easy/Cheat ending
        if(easyMode || cheatMode.isActivated)
            return 0;
        //No Hit ending
        if(!hasBeenHit)
            return 3;
        //Flawless ending
        if(flawlessMode)
            return 2;
        //Normal ending
        return 1;
    }
}
