using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Simple : Projectile {

    void FixedUpdate () {
        transform.position = transform.position + direction * GetMoveSpeed() * Time.deltaTime;
    }
}
