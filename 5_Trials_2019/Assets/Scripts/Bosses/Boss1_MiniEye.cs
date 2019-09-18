using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_MiniEye : MonoBehaviour
{
    public bool isEyeOpen;
    public bool isActing;
    private Animator animator;

    public GameObject projectile;
    public GameObject player;

    public int shootTimer;
    private int shootTimerCount;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEyeOpen | isActing)
            return;

        shootTimerCount++;

        if(shootTimerCount >= shootTimer)
        {
            shoot();
            shootTimerCount = 0;
        }
    }

    public void setShootTimer(int startTime, int timer)
    {
        shootTimerCount = startTime;
        shootTimer = timer;
    }

    public void shoot()
    {
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

        //Calculates the direction of the player
        Vector2 direction = player.transform.position - gameObject.transform.position;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile_Simple>().direction = direction;
    }

    //Toggle the eye
    public void openEye()
    {
        this.isEyeOpen = !isEyeOpen;
        openEye(isEyeOpen);
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

    public void chargeEye(bool isCharging)
    {
        isActing = true;
        animator.SetTrigger("Charging_Open");
    }
}
