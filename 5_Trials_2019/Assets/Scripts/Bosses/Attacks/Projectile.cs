using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Obj_Projectile objProjectile;
    public Vector3 direction;
    private float lifeTime;

    void Start()
    {
        lifeTime = objProjectile.lifeTime;
    }

    void Update ()
    {
        //Removes the object after a set amount of time
        lifeTime = lifeTime - Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }

        //Removes the object after reaching a certain distance
        if (transform.position.x > objProjectile.killDistance.x | transform.position.x < -objProjectile.killDistance.x)
            Destroy(gameObject);
        if (transform.position.y > objProjectile.killDistance.y | transform.position.y < -objProjectile.killDistance.y)
            Destroy(gameObject);
    }

    public int GetDamage()
    {
        return objProjectile.damage;
    }

    public float GetMoveSpeed()
    {
        return objProjectile.moveSpeed;
    }

}
