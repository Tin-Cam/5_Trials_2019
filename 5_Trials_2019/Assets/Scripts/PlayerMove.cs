using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rig;
    Animator animator;

    public GameManager gameManager;
    public Joystick joystick;
    public bool joystickControls;

    [Space(15)]
    public float defaultKnockBack;
    public Vector2 roomBounds;

    [Space(15)]
    public float moveSpeed;

    public bool canMove;
    public bool isMoving;

    public byte direction;
    // Direction uses an int value to determine which direction the player is facing
    // 0:   Up
    // 1:   Left
    // 2:   Down
    // 3:   Right


    void Start() {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (!canMove)
        {
            rig.velocity = new Vector3(0f, 0f, 0f);
            return;
        }

        float inputX = GetInput("Horizontal");
        float inputY = GetInput("Vertical");

        MoveX(inputX);
        MoveY(inputY);

        if (rig.velocity.Equals(Vector3.zero))
            isMoving = false;
        else
            isMoving = true;

        animator.SetBool("Moving", isMoving);

        if (!isMoving)
            animator.SetTrigger("Idle");
    }

    //Handles horizontal movement
    private void MoveX(float input)
    {
        //Handles moving right
        if (input > 0.2f)
        {
            rig.velocity = new Vector3(moveSpeed, rig.velocity.y, 0f);
            SetDirection(3);
        }
        //Handles moving left
        else if (input < -0.2f)
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
    private void MoveY(float input)
    {
        //Handles moving up
        if (input > 0.2f)
        {
            rig.velocity = new Vector3(rig.velocity.x, moveSpeed, 0f);
            SetDirection(0);
        }
        //Handles moving down
        else if (input < -0.2f)
        {
            rig.velocity = new Vector3(rig.velocity.x, -moveSpeed, 0f);
            SetDirection(2);
        }
        else
        {
            rig.velocity = new Vector3(rig.velocity.x, 0f, 0f);
        }
    }

    private float GetInput(string axis)
    {
        //Returns standard Unity input
        if (!joystickControls)
            return Input.GetAxisRaw(axis);

        //Returns stick input
        if (axis == "Horizontal")
            return joystick.Horizontal;
        else if (axis == "Vertical")
            return joystick.Vertical;
        else
            Debug.LogError("GetInput: Given string is not an input!");
        return 0;
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

    //Pushes the player in a given direction
    public IEnumerator knockBack(Vector2 direction, float intensity)
    {
        for (int i = 0; i < 5; i++)
        {
            rig.AddForce(direction * intensity);           
            yield return new WaitForFixedUpdate();
            CheckBounds();
        }     
    }

    //Code to ensure player doesn't get pushed out of the room
    private void CheckBounds()
    {
        
        float objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        float objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, roomBounds.x * -1 - objectWidth, roomBounds.x + objectWidth);
        position.y = Mathf.Clamp(position.y, roomBounds.y * -1 - objectHeight, roomBounds.y + objectHeight);
        transform.position = position;
    }

    
    
}
