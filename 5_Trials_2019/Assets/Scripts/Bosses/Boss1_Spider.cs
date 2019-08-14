using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Spider : _BossBase
{

    Vector2 movement;
    public float speed;

    public float maxMoveTime;
    private float moveTime;

    public float minX;
    public float maxX;


    // Start is called before the first frame update
    override protected void BossStart()
    {
        movement = new Vector2(1, 0);        
    }

    // Update is called once per frame
    void Update()
    {
        moveTime++;

        //Changes boss' direcection after reaching a set point
        if (rig.position.x <= minX)
        {
            movement = new Vector2(Mathf.Abs(movement.x), 0);
            moveTime = 0;
        }

        if (rig.position.x >= maxX)
        {
            movement = new Vector2(Mathf.Abs(movement.x) * -1, 0);
            moveTime = 0;
        }
    }

    void FixedUpdate()
    {
        move();
    }

    void move()
    {
        rig.MovePosition(rig.position + movement * speed * Time.fixedDeltaTime);
    }
}
