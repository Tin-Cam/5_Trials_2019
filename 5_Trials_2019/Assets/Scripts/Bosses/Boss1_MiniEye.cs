using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_MiniEye : MonoBehaviour
{
    public bool isEyeOpen;

    public GameObject projectile;
    public GameObject player;

    public int shootTimer;
    private int shootTimerCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEyeOpen)
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
        isEyeOpen = !isEyeOpen;
        //animator.SetBool("isOpen", isEyeOpen);
    }

    //Sets the state of the eye
    public void openEye(bool isEyeOpen)
    {
        this.isEyeOpen = isEyeOpen;
        //animator.SetBool("isOpen", isEyeOpen);
    }

    void chargeEye(bool isCharging)
    {
        openEye(isCharging);
        //animator.SetBool("isCharging", isCharging);
    }
}
