using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bomb : Projectile
{
    public Projectile projectile;
    public int projectileAmount;

    public float fuse;

    private bool hasBeenHit = false;

    // Update is called once per frame
    void Update()
    {
        if(!hasBeenHit)
            fuse = fuse - Time.deltaTime;

        if (fuse <= 0)
            Explode();
    }

    private void Explode()
    {
        float angle = 360 / projectileAmount;

        for (int i = 0; i < projectileAmount; i++)
        {
            Quaternion shootAngle = Quaternion.AngleAxis(angle * i, Vector3.forward);

            Projectile tempProjectile;
            tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

            Vector3 direction = shootAngle * Vector3.one;
            direction.Normalize();

            tempProjectile.direction = direction;
        }
        AudioManager.instance.Play("Crash");
        Destroy(gameObject);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.tag == "Sword"){
    //         hasBeenHit = true;
    //         AudioManager.instance.Play("Discard", 0.75f, 0.75f);
    //         Destroy(gameObject);
    //     }
    // }
}
