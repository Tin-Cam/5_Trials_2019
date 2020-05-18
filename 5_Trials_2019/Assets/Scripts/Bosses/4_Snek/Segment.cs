using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Obj_Segment segmentRef;

    public bool isHead;
    public bool canShoot;


    private int health;
    private bool isDestroyed = false;

    private Boss4_Controller controller;
    private AudioManager audioManager;
    private SpriteRenderer render;

    private float initialG;
    private float initialB;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        health = segmentRef.health;

        render = GetComponent<SpriteRenderer>();

        initialG = render.color.g;
        initialB = render.color.b;

    }

    public void Shoot(Vector3 target)
    {
        if (!canShoot || isDestroyed)
            return;


        audioManager.Play("Boss_Shoot");
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
        if (other.tag != "Sword")
            return;

        if (isDestroyed)
            return;

        controller.SegmentHurt();

        if (isHead)
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

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    public void SetController(Boss4_Controller controller)
    {
        this.controller = controller;
    }
}
