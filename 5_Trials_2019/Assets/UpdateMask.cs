using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteMask))]
public class UpdateMask : MonoBehaviour
{
    public SpriteRenderer reference;
    private SpriteMask mask;

    void Start()
    {
        reference = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mask.sprite = reference.sprite;
    }
}
