using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Spider : _BossBase
{

    //Movement Variables
    [Space(15)]
    public float speed;
    Vector2 movement;

    public float moveRange;
    private float moveTime = 0;


    //Eye Variables
    [Space(15)]
    public bool isEyeOpen;
    public Animator animator;

    public float maxEyeTime;
    private float eyeTime;


    //Attack Variables
    [Space(15)]
    public GameObject projectile;
    public GameObject player;

    public float attackFrequency;
    private float attackFrequencyTime;

    // Start is called before the first frame update
    override protected void BossStart()
    {
        movement = new Vector2(rig.position.x, rig.position.y);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        attack();
        eyeUpdate();
        moveUpdate();
    }

    void attack()
    {
        if (isEyeOpen)
        {
            attackFrequencyTime++;
                if (attackFrequencyTime >= attackFrequency) {
                    //Creates the projectile
                    GameObject tempProjectile;
                    tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

                    //Calculates the direction of the player
                    Vector2 direction = player.transform.position - gameObject.transform.position;
                    direction.Normalize();

                    //'Fires' the projectile
                    tempProjectile.GetComponent<Projectile_Simple>().direction = direction;

                attackFrequencyTime = 0;
            }
        }
    }

    void eyeUpdate()
    {
        eyeTime++;

        if(eyeTime >= maxEyeTime)
        {
            openEye();
            eyeTime = 0;
        }
    }

    //Toggle the eye
    void openEye()
    {
        isEyeOpen = !isEyeOpen;
        animator.SetBool("isOpen", isEyeOpen);
    }

    //Sets the state of the eye
    void openEye(bool isEyeOpen)
    {
        this.isEyeOpen = isEyeOpen;
        animator.SetBool("isOpen", isEyeOpen);
    }

    override protected void playerAttacked()
    {
        if (isEyeOpen)
            takeDamage(1);
    }

    //Updates the triggers for movement
    void moveUpdate()
    {
        moveTime++;
        movement = new Vector2(Mathf.Sin(moveTime * Mathf.Deg2Rad) * moveRange, movement.y);
    }

    void FixedUpdate()
    {
        _move();
    }

    void _move()
    {
        rig.MovePosition(movement * new Vector2(speed * Time.fixedDeltaTime, 1));
      
    }
}
