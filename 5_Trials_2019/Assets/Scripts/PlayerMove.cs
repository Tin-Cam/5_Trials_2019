using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rig;
    SpriteRenderer render;
    Animator animator;

    public GameManager gameManager;
    public bool godMode;

    [Space(15)]
    public float defaultKnockBack;
    public bool isInvincible;
    public float flashAmount;
    public float flashSpeed = 1;

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
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

    //Pushes the player in a given direction
    public IEnumerator knockBack(Vector2 direction, float intensity)
    {
        for (int i = 0; i < 5; i++)
        {
            rig.AddForce(direction * intensity);
            yield return new WaitForFixedUpdate();
        }
    }

    //Makes the player invincible; used when they're hit
    public IEnumerator invincible()
    {
        

        isInvincible = true;
        for (int i = 0; i < flashAmount; i++)
        {
            flashRed();
            yield return new WaitForSeconds(flashSpeed);
        }
        render.color = Color.white;
        isInvincible = false;
    }

    //Alternates player's color from white to red when hit
    private void flashRed()
    {
        if (render.color.Equals(Color.red))
        {
            render.color = Color.white;
            return;
        }

        if (render.color.Equals(Color.white))
        {
            render.color = Color.red;
            return;
        }
    }

    //Methods for player getting hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (godMode | isInvincible)
            return;

        if (other.tag == "Projectile")
        {
            gameManager.PlayerTakeDamage(1);
            Vector2 direction = transform.position - other.transform.position;
           
            StartCoroutine(knockBack(direction, defaultKnockBack));
            StartCoroutine(invincible());
        }
    }
}
