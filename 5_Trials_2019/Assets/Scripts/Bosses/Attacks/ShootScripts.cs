using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScripts : MonoBehaviour
{
    public GameObject bulletRef;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
    }

    public GameObject Shoot(Vector3 target)
    {
        return Shoot(bulletRef, target);
    }

    public GameObject Shoot(Quaternion angle)
    {
        return Shoot(bulletRef, angle);
    }

    public GameObject Shoot(GameObject bullet, Vector3 target)
    {
        audioManager.Play("Boss_Shoot");
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(bullet, transform.position, transform.rotation);

        //Calculates the direction of the target
        Vector2 direction = target - gameObject.transform.position;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile>().direction = direction;

        return tempProjectile;
    }

    public GameObject Shoot(GameObject bullet, Quaternion angle)
    {
        audioManager.Play("Boss_Shoot");
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(bullet, transform.position, transform.rotation);

        Vector2 direction = new Vector2(1, 0);
        direction = angle * direction;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile>().direction = direction;

        return tempProjectile;
    }

}
