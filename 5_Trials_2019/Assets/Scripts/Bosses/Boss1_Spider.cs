using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Spider : _BossBase
{

    //Movement Variables
    Vector2 movement;
    public float speed;

    public float minX;
    public float maxX;

    //Eye Variables
    public bool isOpen;
    public Animator animator;

    public float maxEyeTime;
    private float eyeTime;

    //Attack Variables
    public GameObject projectile;
    public GameObject player;

    public float attackFrequency;
    private float attackFrequencyTime;

    // Start is called before the first frame update
    override protected void BossStart()
    {
        movement = new Vector2(1, 0);
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
        if (isOpen)
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
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }

    //Sets the state of the eye
    void openEye(bool isOpen)
    {
        this.isOpen = isOpen;
        animator.SetBool("isOpen", isOpen);
    }

    //Updates the triggers for movement
    void moveUpdate()
    {

        //Changes boss' direcection after reaching a set point
        if (rig.position.x <= minX)
        {
            movement = new Vector2(Mathf.Abs(movement.x), 0);
        }

        if (rig.position.x >= maxX)
        {
            movement = new Vector2(Mathf.Abs(movement.x) * -1, 0);
        }
    }

    void FixedUpdate()
    {
        _move();
    }

    void _move()
    {
        rig.MovePosition(rig.position + movement * speed * Time.fixedDeltaTime);
    }
}
