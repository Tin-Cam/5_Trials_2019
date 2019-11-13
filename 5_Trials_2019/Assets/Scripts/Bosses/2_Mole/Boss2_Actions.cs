using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Actions : _ActionBase
{
    public float idleTime;

    private Boss2_Move move;
    private Boss2_Controller controller;
    private GameObject player;
    private Animator animator;

    public GameObject projectile;

    public void Init()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<Boss2_Controller>();
        move = GetComponent<Boss2_Move>();

        player = controller.player;
        isActing = false;

        actionList.Add("Idle");
        actionList.Add("MoveRandom");
        actionList.Add("Attack");

        actionList.Add("Desperation");
    }

    void Shoot(float offset)
    {
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

        //Calculates the direction of the player (and applies an offset to it)
        Vector2 direction = player.transform.position - gameObject.transform.position;
        Quaternion offsetVector = Quaternion.AngleAxis(offset, Vector3.forward);

        direction = offsetVector * direction;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile_Simple>().direction = direction;
    }

    //ACTIONS ---------------------------------

    //Action 0 - Idle
    private IEnumerator Idle()
    {
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(idleTime);
    }

    //Action 1 - Move randomly across the room
    private IEnumerator MoveRandom()
    {


        for(int i = 0; i < 3; i++)
            yield return move.MovePosition();
    
        Debug.Log("Move Done");
        yield return new WaitForEndOfFrame();
    }

    //Action 2 - Attack
    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        Shoot(0);
        Shoot(30);
        Shoot(-30);
        yield return new WaitForSeconds(1);
    }

    //Action ? - Desperation
    private IEnumerator Desperation()
    {
        animator.SetTrigger("Desperation");
        Shoot(0);
        Shoot(30);
        Shoot(-30);
        yield return new WaitForSeconds(1);
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
