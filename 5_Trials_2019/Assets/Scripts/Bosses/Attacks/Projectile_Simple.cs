using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Simple : Projectile {

    public Vector3 direction;

    void FixedUpdate () {
        transform.position = transform.position + direction * moveSpeed * Time.deltaTime;
    }
}
