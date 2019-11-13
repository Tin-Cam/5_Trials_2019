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
    public IEnumerator MovePosition()
    {
        int rng = Random.Range(0, holes.Capacity);
        yield return MovePosition(rng);
    }


    //Waits until boss is underground, then changes its position
    public IEnumerator MovePosition(int position)
    {
        //Waits for the boss to dig and move
        animator.SetTrigger("Dig");       
        yield return WaitForAnimation("Boss2_Digging");
        transform.position = holes[position].position;
        yield return WaitForAnimation("Boss2_Underground");
        yield return WaitForAnimation("Boss2_Rising");

        yield return new WaitForSeconds(holdTime);

    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }

    //Returns true when an animation stops playing
    public IEnumerator WaitForAnimation(string animation)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Waiting for " + animation);
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
