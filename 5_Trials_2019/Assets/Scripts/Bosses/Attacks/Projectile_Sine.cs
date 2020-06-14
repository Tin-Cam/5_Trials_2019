using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Sine : Projectile
{
    public GameObject projectileRef;

    private GameObject projectile1;
    private GameObject projectile2;

    public bool doubleProjectiles;

    public float waveSpeed;
    public float waveAmplitude;

    private float t;

    void Start()
    {
        projectile1 = Instantiate(projectileRef, transform.position, transform.rotation, transform);
  
        if (doubleProjectiles)
            projectile2 = Instantiate(projectileRef, transform.position, transform.rotation, transform);
    }

    void FixedUpdate()
    {
        transform.position = transform.position + direction * GetMoveSpeed() * Time.deltaTime;

        projectile1.transform.localPosition = CalculateSineMovement(projectile1.transform, false);
        if (doubleProjectiles)
            projectile2.transform.localPosition = CalculateSineMovement(projectile2.transform, true);

        t += Time.deltaTime;
    }

    private Vector3 CalculateSineMovement(Transform projectile, bool invert)
    {
        
        float x = waveAmplitude * Mathf.Sin(waveSpeed * t);

        if (invert)
            x = -x;

        Vector3 result = new Vector3(x, 0, 0);

        return result;
    }
}
