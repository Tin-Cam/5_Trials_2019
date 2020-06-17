using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Simple : Projectile {

    void FixedUpdate () {
        transform.localPosition = transform.localPosition + direction * GetMoveSpeed() * Time.deltaTime;
    }
}
