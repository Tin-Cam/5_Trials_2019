using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Actions : _ActionBase
{
    private Boss1_Move move;
    private Boss1_Controller controller;
    private Boss1_Eyes eyes;
    private AudioManager audioManager;

    private GameObject player;

    //Attack Variables
    [Space(15)]
    public GameObject projectile;
    

    //Action Variables
    [Space(15)]
    public float exposeEyeTime;

    public float shortAttackAmount;
    public float shortAttackFrequency;

    public float longAttackAmount;
    public float longAttackFrequency;

    public float chargeTime;

    public float desperationAmount;
    public float desperationFrequency;

    


    public void Init()
    {
        controller = GetComponent<Boss1_Controller>();
        move = GetComponent<Boss1_Move>();
        eyes = GetComponent<Boss1_Eyes>();
        audioManager = controller.audioManager;

        player = controller.player;
        isActing = false;

        //Set actions
        actionList.Add("exposeEye");
        actionList.Add("attackShort");
        actionList.Add("attackLong");
        actionList.Add("attackDesperation");
    }

    void Shoot()
    {
        audioManager.Play("Boss_Shoot");

        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

        //Calculates the direction of the player
        Vector2 direction = player.transform.position - gameObject.transform.position;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile_Simple>().direction = direction;
    }

    //ACTIONS ---------------------------------

    //Action 0 - Expose Eye (No attacking)
    private IEnumerator exposeEye()
    {
        eyes.openEye(true);
        move.SetIsMoving(false);
        yield return new WaitForSeconds(exposeEyeTime);
        controller.DefaultState();
    }

    //Action 1 - Short Attack
    private IEnumerator attackShort()
    {
        eyes.openEye(true);
        for (int i = 0; i < shortAttackAmount; i++)
        {
            yield return new WaitForSeconds(shortAttackFrequency);
            Shoot();
        }
        controller.DefaultState();
    }

    //Action 2 - Long Attack
    private IEnumerator attackLong()
    {

        eyes.chargeEye(true);
        yield return new WaitForSeconds(chargeTime);


        eyes.openEye(true);
        for (int i = 0; i < longAttackAmount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(longAttackFrequency);
        }
        controller.DefaultState();
    }

    //Action 3 - Desperation Attack
    private IEnumerator attackDesperation()
    {
        move.SetIsMoving(false);
        eyes.chargeEye(false);
        eyes.chargeMiniEyes();
        ShowDesperationFilter(true);

        yield return new WaitForSeconds(chargeTime);

        eyes.openEye(true);
        eyes.openMiniEyes(true);

        for (int i = 0; i < desperationAmount; i++)
        {
            Shoot();
            eyes.shootMiniEyes();
            yield return new WaitForSeconds(desperationFrequency);
        }

        controller.DefaultState();
    }

    public override void DefaultState()
    {
        StopActing();
        ShowDesperationFilter(false);
    }
}
