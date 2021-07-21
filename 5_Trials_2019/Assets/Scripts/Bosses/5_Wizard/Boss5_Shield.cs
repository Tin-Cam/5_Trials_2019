using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Shield : MonoBehaviour
{
    public bool useShield;
    public bool isShieldActive;
    public bool isRecharging;

    public GameObject shield;

    public int maxRechargeCount;
    private int rechargeCounter;

    public float maxShieldTime;
    private float shieldTimer;

    private Animator shieldAnimator;

    public void Init()
    {
        ShieldActive(false);
        shieldAnimator = shield.gameObject.GetComponent<Animator>();
    }

    //Timer for shield when active
    public void FixedUpdate()
    {
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;

            if (shieldTimer <= 0){
                AudioManager.instance.Play("Shield_Down");
                ShieldActive(false);
            }
        }
    }

    public void ShieldActive(bool isActive)
    {
        isShieldActive = isActive;
        shield.SetActive(isActive);

        isRecharging = false;

        if (isActive)
        {
            AudioManager.instance.Play("Shield_Up");
            shieldAnimator.SetTrigger("Activate");
            shieldTimer = maxShieldTime;
        }
    }

    public int DecreaseRecharge()
    {
        if (!isRecharging)
            return 99;

        rechargeCounter--;
        return rechargeCounter;
    }

    //If boss is damaged, makes sure recharge is active; 
    public void BossHit()
    {
        if (!useShield)
            return;
        if (isShieldActive)
            return;
        if (isRecharging)
            return;

        //Activates recharge if it isn't active
        rechargeCounter = Random.Range(2, maxRechargeCount);
        isRecharging = true;
    }
}
