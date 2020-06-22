using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Shield : MonoBehaviour
{
    public bool useShield;
    public bool isShieldActive;
    public bool isRecharging;

    public SpriteRenderer shieldRender;

    private int rechargeCounter;
    private float shieldTimer;

    public void Init()
    {
        ShieldActivated(false);
    }

    public void ShieldActivated(bool isActive)
    {
        isShieldActive = isActive;
        shieldRender.enabled = isActive;

        isRecharging = false;
    }

    public int IncrementRecharge()
    {
        if (!isRecharging)
            return 0;

        rechargeCounter++;
        return rechargeCounter;
    }
}
