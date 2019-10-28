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

    //Picks a position to go to
    public void MovePosition(int position)
    {
        animator.SetTrigger("Dig");
        transform.position = holes[position].position;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
