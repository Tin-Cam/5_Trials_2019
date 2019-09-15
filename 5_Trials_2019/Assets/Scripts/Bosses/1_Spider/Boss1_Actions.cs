using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Actions : _ActionBase
{
    private Boss1_Move move;
    private Boss1_Controller controller;
    private Boss1_Eyes eyes;

    private GameObject player;

    //Attack Variables
    [Space(15)]
    public GameObject projectile;
    public bool isActing;

    //Action Variables
    [Space(15)]
    public float eyeTime;
    public float attackFrequency;
    public float chargeTime;

    private int maxAction;


    public void Init()
    {
        controller = GetComponent<Boss1_Controller>();
        move = GetComponent<Boss1_Move>();
        eyes = GetComponent<Boss1_Eyes>();

        player = controller.player;
        isActing = false;

        //Set actions
        maxAction = 3;
        actionList.Add("exposeEye");
        actionList.Add("attackShort");
        actionList.Add("attackLong");
        actionList.Add("attackDesperation");
    }

    void Shoot()
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

    //ACTIONS ---------------------------------

    //Action 0 - Expose Eye (No attacking)
    private IEnumerator exposeEye()
    {
        eyes.openEye(true);
        move.isMoving = false;
        yield return new WaitForSeconds(eyeTime);
        eyes.openEye(false);
        move.isMoving = true;
        isActing = false;
    }

    //Action 1 - Short Attack
    private IEnumerator attackShort()
    {
        eyes.openEye(true);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < attackFrequency; j++)
                yield return new WaitForEndOfFrame();
            Shoot();
        }
        eyes.openEye(false);
        isActing = false;
    }

    //Action 2 - Long Attack
    private IEnumerator attackLong()
    {

        eyes.openEye(true);
        eyes.chargeEye(true);
        yield return new WaitForSeconds(chargeTime);


        eyes.openEye(true);
        for (int i = 0; i < 30; i++)
        {
            Shoot();
            for (int j = 0; j < 10; j++)
                yield return new WaitForEndOfFrame();
        }
        eyes.openEye(false);
        //animator.ResetTrigger("Charging");
        eyes.chargeEye(false);
        isActing = false;
    }

    //Action 3 - Desperation Attack
    private IEnumerator attackDesperation()
    {
        move.isMoving = false;
        eyes.openEye(false);
        eyes.chargeEye(true);
        eyes.chargeMiniEyes(true);


        yield return new WaitForSeconds(chargeTime);

        eyes.openEye(true);

        for (int i = 0; i < 30; i++)
        {
            Shoot();
            eyes.shootMiniEyes();
            for (int j = 0; j < 10; j++)
                yield return new WaitForEndOfFrame();
        }


        eyes.openEye(false);
        eyes.chargeEye(false);
        eyes.chargeMiniEyes(false);
        move.isMoving = true;
        isActing = false;
    }
}
