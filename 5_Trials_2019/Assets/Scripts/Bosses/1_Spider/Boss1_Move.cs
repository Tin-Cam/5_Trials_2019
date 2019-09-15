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

    public void Init()
    {
        rig = GetComponent<Rigidbody2D>();
        controller = GetComponent<Boss1_Controller>();   

        movement = new Vector2(rig.position.x, rig.position.y);
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
            return;

        moveTime += 1 * (speed / 10);
        movement = new Vector2(Mathf.Sin(moveTime * Mathf.Deg2Rad) * moveRange, movement.y);
    }

    void FixedUpdate()
    {
        if (!isMoving)
            return;
        rig.MovePosition(movement * new Vector2(Time.fixedDeltaTime, 1));
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }
}
