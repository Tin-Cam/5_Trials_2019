using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteMask))]
public class UpdateMask : MonoBehaviour
{
    public bool referenceThisObject = true;
    public SpriteRenderer reference;
    private SpriteMask mask;

    void Start()
    {
        mask = GetComponent<SpriteMask>();
        if (referenceThisObject)            
            reference = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mask.sprite = reference.sprite;
    }
}
