using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Eyes : MonoBehaviour
{
    private Boss1_Actions action;
    private Boss1_Controller controller;
    private Animator animator;

    //Eye Variables
    [Space(15)]
    public bool isEyeOpen;

    [Space(15)]
    public bool miniEyesOpen;
    public int miniTimer;
    private int miniTimercount;

    public int miniEyeStunTime;
    public Boss1_MiniEye miniEyeR;
    public Boss1_MiniEye miniEyeL;

    private bool nextMiniEye; //Dertermines which mini eye to shoot next (false = left, true = right)

    public void Init()
    {
        controller = GetComponent<Boss1_Controller>();
        action = GetComponent<Boss1_Actions>();
        animator = GetComponent<Animator>();

        openEye(false);
        openMiniEyes(false);

        miniEyeL.stunTime = miniEyeStunTime;
        miniEyeR.stunTime = miniEyeStunTime;

        GameObject player = controller.player;

        miniEyeL.player = player;
        miniEyeR.player = player;

        StartCoroutine(ShootCycle());
    }

    //Periodically makes a mini eye shoot. (Mini eye won't shoot if it's closed)
    public IEnumerator ShootCycle()
    {
        while(Time.timeScale == 0)
            yield return new WaitForFixedUpdate();

        yield return new WaitForFixedUpdate();
        miniTimercount++;

        if (miniTimercount < miniTimer)
        {
            StartCoroutine(ShootCycle());
            yield break;
        }

        Boss1_MiniEye miniEye;

        if (!nextMiniEye)
            miniEye = miniEyeL;
        else
            miniEye = miniEyeR;

        nextMiniEye = !nextMiniEye;

        miniEye.shoot();
        miniTimercount = 0;

        StartCoroutine(ShootCycle());
    }

    //MAIN EYE -----------------------------------

    //Sets the state of the eye
    public void openEye(bool isEyeOpen)
    {
        this.isEyeOpen = isEyeOpen;      
        if (isEyeOpen)
            animator.SetTrigger("Open");
        else if (!isEyeOpen)
            animator.SetTrigger("Closed");
    }

    //Sets the eye to the charging animation
    public void chargeEye(bool isEyeOpen)
    {
        animator.ResetTrigger("Open");
        animator.ResetTrigger("Closed");

        this.isEyeOpen = isEyeOpen;
        AudioManager.instance.Play("Boss_Charge");
        if (isEyeOpen)
            animator.SetTrigger("Charging_Open");
        else if (!isEyeOpen)
            animator.SetTrigger("Charging_Closed");
    }

    //MINI EYES ----------------------------------

    //Shoots both mini eyes regardless of their state
    public void shootMiniEyes()
    {
        miniEyeL.ForceShoot();
        miniEyeR.ForceShoot();
    }

    //Starts the timers for the mini eyes. Eyes will alternate shots
    public void setMiniEyeTimer(int timer)
    {
        miniTimer = timer;      
        openMiniEyes(true);
    }

    public void openMiniEyes(bool isOpen)
    {
        miniTimercount = 0;
        miniEyeL.openEye(isOpen);
        miniEyeR.openEye(isOpen);
    }

    public void chargeMiniEyes()
    {
        miniEyeL.chargeEye();
        miniEyeR.chargeEye();
    }

    public void DefaultState()
    {
        openEye(false);
        

        if (miniEyeL.isEyeOpen & miniEyeL.isActing)
        {
            miniEyeL.isActing = false;
            miniEyeR.isActing = false;
            openMiniEyes(true);
        }
    }
}
