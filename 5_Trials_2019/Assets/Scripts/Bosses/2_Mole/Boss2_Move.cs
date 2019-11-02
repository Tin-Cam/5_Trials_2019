using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Move : _MoveBase
{
    public float holdTime;
    public List<Transform> holes = new List<Transform>();

    private Rigidbody2D rig;
    private Animator animator;
    private Boss2_Actions action;

    public void Init()
    {        
        animator = GetComponent<Animator>();
        action = GetComponent<Boss2_Actions>();
    }

    //Randomly chooses a position to jump to
    public void MovePosition(int times)
    {
        int rng = Random.Range(0, holes.Capacity);
        MovePosition(times, rng);
    }

    //Picks a position to go to and starts moving to it
    public void MovePosition(int times, int position)
    {       
        StartCoroutine(MovingPosition(times, position));
    }

    //Waits until boss is underground, then changes its position
    private IEnumerator MovingPosition(int times,  int position)
    {
        animator.SetTrigger("Dig");
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Boss2_Digging"))
        {
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Digging"));
        transform.position = holes[position].position;

        yield return new WaitForSeconds(holdTime);

        //Rerun function if times is greater than 0
        times--;
        if (times > 0)
            MovePosition(times);

    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
