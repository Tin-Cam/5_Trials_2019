using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    private PlayerMove playerMove;

    public bool canAttack;
    public GameObject sword;
    public float holdTime;

    public Animator animator;

    void Start()
    {
        audioManager = AudioManager.instance;
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
            audioManager.Play("Player_Attack");
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

        DefaultState();
    }

    public void DefaultState()
    {
        animator.SetBool("Attack", false);
        canAttack = true;
        playerMove.canMove = true;
        StartCoroutine(ResetSword());
    }

    private IEnumerator ResetSword()
    {
        yield return new WaitForEndOfFrame();
        sword.SetActive(false);
        sword.transform.position = Vector3.zero;
    }
}
