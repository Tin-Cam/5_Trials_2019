using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public bool isCut = false;
    public Sprite cutBush;

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

        isCut = true;
    }

}