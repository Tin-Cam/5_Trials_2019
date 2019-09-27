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
    }

    void Update()
    {
        miniTimercount++;

        if (miniTimercount < miniTimer)
            return;

        Boss1_MiniEye miniEye;

        if (!nextMiniEye)
            miniEye = miniEyeL;
        else
            miniEye = miniEyeR;

        nextMiniEye = !nextMiniEye;

        miniEye.shoot();
        miniTimercount = 0;
    }

    //MAIN EYE -----------------------------------

    //Toggle the eye
    public void openEye()
    {
        isEyeOpen = !isEyeOpen;
        animator.SetBool("isOpen", isEyeOpen);
    }

    //Sets the state of the eye
    public void openEye(bool isEyeOpen)
    {
        this.isEyeOpen = isEyeOpen;      
        if (isEyeOpen)
            animator.SetTrigger("Open");
        else if (!isEyeOpen)
            animator.SetTrigger("Closed");
    }

    public void chargeEye(bool isEyeOpen)
    {
        animator.ResetTrigger("Open");
        animator.ResetTrigger("Closed");

        this.isEyeOpen = isEyeOpen;
        if (isEyeOpen)
            animator.SetTrigger("Charging_Open");
        else if (!isEyeOpen)
            animator.SetTrigger("Charging_Closed");
    }

    //MINI EYES ----------------------------------

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
