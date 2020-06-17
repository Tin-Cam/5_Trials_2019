using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Spin : Projectile
{
    public Projectile projectile;
    public int projectileAmount;

    public float startingSpeed;
    public float speedCap;
    public float rotationAcceleration;
    private float speed;

    public bool antiClockwise;

    void Start()
    {
        if (!antiClockwise)
        {
            startingSpeed = -startingSpeed;
            rotationAcceleration = -rotationAcceleration;
        }

        speed = startingSpeed;

        SpawnProjectiles();
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);

        if(Mathf.Abs(speed) < speedCap)
            speed += rotationAcceleration;
    }

    private void SpawnProjectiles()
    {
        float angle = 360 / projectileAmount;

        for (int i = 0; i < projectileAmount; i++)
        {
            Quaternion shootAngle = Quaternion.AngleAxis(angle * i, Vector3.forward);

            Projectile tempProjectile;
            tempProjectile = Instantiate(projectile, transform.position, transform.rotation, transform);

            Vector3 direction = shootAngle * Vector3.one;
            direction.Normalize();

            tempProjectile.direction = direction;
        }       
    }
}
