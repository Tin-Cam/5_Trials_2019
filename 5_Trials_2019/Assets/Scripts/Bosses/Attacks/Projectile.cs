using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage;
    public float moveSpeed;
    public float lifeTime;
    public Vector2 killDistance;

    void Update ()
    {
        //Removes the object after a set amount of time
        lifeTime = lifeTime - Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }

        //Removes the object after reaching a certain distance
        if (transform.position.x > killDistance.x | transform.position.x < -killDistance.x)
            Destroy(gameObject);
        if (transform.position.y > killDistance.y | transform.position.y < -killDistance.y)
            Destroy(gameObject);
    }



}
