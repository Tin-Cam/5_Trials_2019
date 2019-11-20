using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Move : _MoveBase
{
    
    public float digSpeed;
    public List<Transform> holes = new List<Transform>();
    private int lastHole = 5;

    private Rigidbody2D rig;
    private Animator animator;
    private Boss2_Actions action;
    private Boss2_Controller controller;

    public void Init()
    {        
        animator = GetComponent<Animator>();
        action = GetComponent<Boss2_Actions>();
        controller = GetComponent<Boss2_Controller>();

        ChangeSpeed(digSpeed);
    }

    //Randomly chooses a position to jump to
    public IEnumerator MovePosition()
    {
        int rng = Random.Range(0, holes.Capacity);

        //Stops the mole from going to the same hole twice
        while(rng == lastHole)
            rng = Random.Range(0, holes.Capacity);

        yield return MovePosition(rng);
    }


    //Waits until boss is underground, then changes its position
    public IEnumerator MovePosition(int position)
    {
        lastHole = position;

        //Waits for the boss to dig and move
        animator.SetTrigger("Dig");       
        yield return WaitForAnimation("Boss2_Digging");
        controller.isHitable = false;

        transform.position = holes[position].position; //Changes the boss' position
        yield return WaitForAnimation("Boss2_Underground");
        controller.isHitable = true;

        yield return WaitForAnimation("Boss2_Rising");
    }

    public override void DefaultState()
    {
        StopAllCoroutines();
    }

    //Finishes when an animation stops playing
    public IEnumerator WaitForAnimation(string animation)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Waiting for " + animation);
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            yield return new WaitForFixedUpdate();
        }
    }

    public void ChangeSpeed(float speed)
    {
        digSpeed = speed;
        animator.SetFloat("Dig_Speed", digSpeed);        
    }
}
