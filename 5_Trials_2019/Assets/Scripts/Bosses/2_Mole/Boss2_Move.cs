using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Move : _MoveBase
{
    public List<Transform> holes = new List<Transform>();

    private Rigidbody2D rig;
    private Animator animator;

    public void Init()
    {        
        animator = GetComponent<Animator>();
    }

    //Randomly chooses a position to jump to
    public void MovePosition()
    {
        int rng = Random.Range(0, holes.Capacity);
        MovePosition(rng);
    }

    //Picks a position to go to and starts moving to it
    public void MovePosition(int position)
    {       
        StartCoroutine(MovingPosition(position));
    }

    //Waits until boss is underground, then changes its position
    private IEnumerator MovingPosition(int position)
    {
        animator.SetTrigger("Dig");
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Boss2_Digging"))
        {
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("Digging"));
        transform.position = holes[position].position;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
