using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Actions : _ActionBase
{
    public float idleTime;

    private Boss2_Move move;
    private Boss2_Controller controller;
    private GameObject player;

    public GameObject projectile;

    public void Init()
    {
        controller = GetComponent<Boss2_Controller>();
        move = GetComponent<Boss2_Move>();

        player = controller.player;
        isActing = false;

        actionList.Add("Idle");
        actionList.Add("MoveRandom");
        actionList.Add("Attack");
    }

    void Shoot(float offset)
    {
        offset = Mathf.Deg2Rad * offset;
        Vector2 offsetVector = new Vector2(Mathf.Cos(offset), -Mathf.Sin(offset));

        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

        //Calculates the direction of the player
        Vector2 direction = player.transform.position - gameObject.transform.position;
        direction.Normalize();
        direction += offsetVector;

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile_Simple>().direction = direction;
    }

    //ACTIONS ---------------------------------

    //Action 0 - Idle
    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);
    }

    //Action 1 - Move randomly across the room
    private IEnumerator MoveRandom()
    {
        move.MovePosition(3);
        Debug.Log("Move Done");
        yield return new WaitForEndOfFrame();
    }

    //Action 2 - Attack
    private IEnumerator Attack()
    {
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
