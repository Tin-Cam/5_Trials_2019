using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public float moveSpeed;
    public float lifeTime;

    void Update ()
    {
        //Removes the object after a set amount of time
        lifeTime = lifeTime - Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
