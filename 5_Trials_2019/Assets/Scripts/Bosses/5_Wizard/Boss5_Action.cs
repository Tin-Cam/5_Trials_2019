using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Action : _ActionBase
{
    public Projectile_Sine projectileSine;
    public float firerate;
    public int burstAmount;

    private GameObject player;
    private Boss5_Controller controller;
    private AudioManager audioManager;

    public void Init()
    {
        controller = GetComponent<Boss5_Controller>();
        audioManager = AudioManager.instance;

        projectileSine.doubleProjectiles = false;

        player = controller.player;

        actionList.Add("ShootSine");
        actionList.Add("ShootSineDouble");
    }

    public IEnumerator ShootSine()
    {
        projectileSine.doubleProjectiles = false;
        GameObject projectile;
        Vector3 target = player.transform.position;

        for(int i = 0; i < burstAmount; i++)
        {
            projectile = Shoot(projectileSine.gameObject, target);
            //Rotates the projectile to face the player
            projectile.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, target - projectile.transform.position), Vector3.forward);
            yield return new WaitForSeconds(firerate);
        }
    }

    public IEnumerator ShootSineDouble()
    {
        projectileSine.doubleProjectiles = true;
        GameObject projectile;
        Vector3 target = player.transform.position;

        for (int i = 0; i < burstAmount; i++)
        {
            projectile = Shoot(projectileSine.gameObject, target);
            //Rotates the projectile to face the player
            projectile.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, target - projectile.transform.position), Vector3.forward);
            yield return new WaitForSeconds(firerate);
        }

        yield break;
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

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
