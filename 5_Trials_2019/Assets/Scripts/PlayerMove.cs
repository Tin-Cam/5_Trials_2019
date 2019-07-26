using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rig;
    SpriteRenderer render;

    public float moveSpeed;
    public Animator animator;

    public bool canMove;
    public bool isMoving;

    public byte direction;
    //Direction uses an int value to determine which direction the player is facing
    // 0:   Up
    // 1:   Left
    // 2:   Down
    // 3:   Right


    void Start() {
        rig = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        if (!canMove)
        {
            rig.velocity = new Vector3(0f, 0f, 0f);
            return;
        }

        MoveX();
        MoveY();

        if (rig.velocity.Equals(Vector3.zero))
            isMoving = false;
        else
            isMoving = true;

        animator.SetBool("Moving", isMoving);
    }

    //Handles horizontal movement
    private void MoveX()
    {
        //Handles moving right
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            rig.velocity = new Vector3(moveSpeed, rig.velocity.y, 0f);
            SetDirection(3);
        }
        //Handles moving left
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            rig.velocity = new Vector3(-moveSpeed, rig.velocity.y, 0f);
            SetDirection(1);
        }
        else
        {
            rig.velocity = new Vector3(0f, rig.velocity.y, 0f);
        }
    }

    //Handles vertical movement
    private void MoveY()
    {
        //Handles moving up
        if (Input.GetAxisRaw("Vertical") > 0f)
        {
            rig.velocity = new Vector3(rig.velocity.x, moveSpeed, 0f);
            SetDirection(0);
        }
        //Handles moving down
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            rig.velocity = new Vector3(rig.velocity.x, -moveSpeed, 0f);
            SetDirection(2);
        }
        else
        {
            rig.velocity = new Vector3(rig.velocity.x, 0f, 0f);
        }
    }

    //Direction uses an int value to determine which direction the player is facing
    // 0:   Up
    // 1:   Left
    // 2:   Down
    // 3:   Right
    private void SetDirection(byte direction)
    {
        this.direction = direction;

        animator.SetFloat("Direction", direction);
    }

    //Returns the player's direction as an angle
    //Up:     0
    //Left:  90
    //Down:   180
    //Right:   270
    public float getDirectionAngle()
    {
        return 90 * direction;
    }
}
