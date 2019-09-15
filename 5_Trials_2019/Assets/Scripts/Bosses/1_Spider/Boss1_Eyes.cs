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
    public Boss1_MiniEye miniEyeR;
    public Boss1_MiniEye miniEyeL;

    public void Init()
    {
        controller = GetComponent<Boss1_Controller>();
        action = GetComponent<Boss1_Actions>();
        animator = GetComponent<Animator>();

        isEyeOpen = false;
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
        animator.SetBool("isOpen", isEyeOpen);
    }

    public void chargeEye(bool isCharging)
    {
        //openEye(isCharging);
        animator.SetBool("isCharging", isCharging);
    }

    //MINI EYES ----------------------------------

    public void shootMiniEyes()
    {
        miniEyeL.shoot();
        miniEyeR.shoot();
    }

    //Starts the timers for the mini eyes. Eyes will alternate shots
    public void setMiniEyeTimer(int timer)
    {
        miniEyeL.setShootTimer(0, timer);
        miniEyeR.setShootTimer(timer / 2, timer);
        openMiniEyes(true);
    }

    public void openMiniEyes(bool isOpen)
    {
        miniEyeL.openEye(isOpen);
        miniEyeR.openEye(isOpen);
    }

    public void chargeMiniEyes(bool isCharging)
    {
        miniEyeL.chargeEye(isCharging);
        miniEyeR.chargeEye(isCharging);
    }
}
