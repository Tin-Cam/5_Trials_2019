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
        mask = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        mask.sprite = reference.sprite;
    }
}
