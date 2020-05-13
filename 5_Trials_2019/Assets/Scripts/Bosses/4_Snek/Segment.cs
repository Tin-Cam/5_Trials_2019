using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public static Vector2 shootBounds = new Vector2(5, 3);

    public bool head;
    public bool canShoot;

    public int health = 5;
    private bool isDestroyed = false;

    private Boss4_Controller controller;

    private SpriteRenderer render;

    private float initialG;
    private float initialB;


    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();

        initialG = render.color.g;
        initialG = render.color.b;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Sword")
            return;

        if (isDestroyed)
            return;

        controller.SegmentHurt();
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


    public void SetController(Boss4_Controller controller)
    {
        this.controller = controller;
    }
}
