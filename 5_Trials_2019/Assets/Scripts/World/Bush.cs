using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public bool isCut = false;
    public Sprite cutBush;
    public GameObject leaves;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isCut)
            return;
        if (other.tag == "Sword")
            CutBush();
    }

    private void CutBush(){
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = cutBush;
        Instantiate(leaves, transform);

        isCut = true;
    }

}