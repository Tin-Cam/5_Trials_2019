using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private PlayerMove playerMove;

    public bool canAttack;
    public GameObject sword;
    public float holdTime;

    public Animator animator;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        sword.SetActive(false);
    }


    void Update()
    {
        if (!canAttack)
            return;

        Attack();
    }

    //Stops the player from moving, then starts the Attack Coroutine
    private void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            canAttack = false;
            playerMove.canMove = false;
            StartCoroutine("AttackCo");
        }
    }

    private IEnumerator AttackCo()
    {
        //Sets the rotation of the sword using the player's direction
        //sword.transform.rotation = Quaternion.Euler(0, 0, playerMove.getDirectionAngle());


        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(holdTime);

        
        animator.SetBool("Attack", false);
        canAttack = true;
        playerMove.canMove = true;
    }
}
