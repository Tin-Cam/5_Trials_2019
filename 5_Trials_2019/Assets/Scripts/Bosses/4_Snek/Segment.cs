using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Obj_Segment segmentRef;

    public bool isEnd;

    public bool canShoot;

    public SpriteRenderer hitSprite;

    private int health;
    private bool isDestroyed = false;

    private Boss4_Controller controller;
    private Boss4_Action action;
    private AudioManager audioManager;
    private SpriteRenderer render;
    private Animator animator;

    private float initialG;
    private float initialB;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;

        health = segmentRef.health;

        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        initialG = render.color.g;
        initialB = render.color.b;

    }

    public IEnumerator StartShoot(Vector3 target)
    {
        if (!canShoot || isDestroyed)
            yield break;

        //Stops from shooting if Segment is outside shootBounds
        if (!CheckShootBounds())
            yield break;

        animator.SetTrigger("Attack");
        yield return WaitForAnimation("Segment_Attack", 2);

        Shoot(target);
    }

    public void Shoot(Vector3 target)
    {
        audioManager.Play(segmentRef.shootSFX);
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(segmentRef.projectile, transform.position, transform.rotation);

        //Calculates the direction of the target
        Vector2 direction = target - gameObject.transform.position;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile>().direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(action.desperation && other.tag == "Player")
        {
            controller.player.GetComponentInChildren<PlayerCollision>().TakeDamge(GetComponent<Collider2D>());
            return;
        }

        if (other.tag != "Sword")
            return;

        if (isDestroyed)
            return;

        if (action.desperation)
        {
            audioManager.Play("Boss_NoDamage");
            return;
        }

        controller.SegmentHurt();
        StartCoroutine(FlashRed());

        if (isEnd)
            return;

        health--;

        if (health <= 0)
            isDestroyed = true;

        ChangeColour();
    }

    public void ChangeColour()
    {
        //If segment not destroyed, make segment more red by 20% of original value
        if (!isDestroyed)
        {
            float newG;
            float newB;

            newG = render.color.g - (initialG * 1 / 5);
            newB = render.color.b - (initialB * 1 / 5);

            Color color = new Color(render.color.r, newG, newB, 1f);

            render.color = color;
        }
        //If segment destroyed, change it to black
        else
        {
            render.color = Color.black;
        }
    }

    //Makes the segment flash red when hit
    private IEnumerator FlashRed()
    {
        
        hitSprite.enabled = true;
        render.enabled = false;
        yield return new WaitForSeconds((float)0.1);
        hitSprite.enabled = false;
        render.enabled = true;
    }

    public bool CheckShootBounds()
    {
        return segmentRef.shootBounds.Contains(transform.position);
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    public void SetController(Boss4_Controller controller)
    {
        this.controller = controller;
    }

    public void SetAction(Boss4_Action action)
    {
        this.action = action;
    }

    //Finishes when an animation stops playing
    public IEnumerator WaitForAnimation(string animation, int layer)
    {
        //Wait for animation to start playing
        while (!animator.GetCurrentAnimatorStateInfo(layer).IsName(animation))
        {
            yield return new WaitForFixedUpdate();
        }

        //Waits for animation to finish
        while (animator.GetCurrentAnimatorStateInfo(layer).IsName(animation))
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
