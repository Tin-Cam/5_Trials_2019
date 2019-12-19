using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss1_Move : _MoveBase
{
    private Rigidbody2D rig;
    private Boss1_Controller controller;

    Vector2 movement;
    public float speed;
    public bool isMoving;
    public float moveRange;
    private float moveTime = 0;
    public Animator legs;

    public void Init()
    {
        Speed = speed;

        rig = GetComponent<Rigidbody2D>();
        controller = GetComponent<Boss1_Controller>();   

        movement = new Vector2(rig.position.x, rig.position.y);
        SetIsMoving(true);
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
            legs.SetFloat("Speed", value * (float)0.05);
        }
    }

    void FixedUpdate()
    {
        if (!isMoving)
            return;

        moveTime += 1 * (speed / 10);
        movement = new Vector2(Mathf.Sin(moveTime * Mathf.Deg2Rad) * moveRange, movement.y);

        rig.MovePosition(movement * new Vector2(Time.fixedDeltaTime, 1));
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
        legs.SetBool("isMoving", value);
    }

    public override void DefaultState()
    {
        SetIsMoving(true);

    }
}
