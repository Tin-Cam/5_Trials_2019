using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Action : _ActionBase
{
    public Projectile_Sine projectileSine;
    public bool doubleProjectiles;
    public float attackDelay;
    public float firerate;
    public int burstAmount;

    public Projectile_Spin projectileSpin;

    public Projectile_Sine projectileDesp1;
    public Projectile_Sine projectileDesp2;
    public float despVariablity;
    public float despFireRate;
    public float despShotAmount;

    private GameObject player;
    private Boss5_Controller controller;
    private AudioManager audioManager;

    public void Init()
    {
        controller = GetComponent<Boss5_Controller>();
        audioManager = AudioManager.instance;

        projectileSine.doubleProjectiles = doubleProjectiles;

        player = controller.player;

        actionList.Add("ShootSine");
        actionList.Add("ShootSpin");

        actionList.Add("ShootDesp1");
        actionList.Add("ShootDesp2");

        actionList.Add("Move");
    }

    public IEnumerator ShootSine()
    {
        if (!doubleProjectiles)
            controller.bossAnimator.SetTrigger("Shoot_Single");
        else
            controller.bossAnimator.SetTrigger("Shoot_Double");

        projectileSine.doubleProjectiles = doubleProjectiles;
        GameObject projectile;
        Vector3 target = player.transform.position;

        yield return new WaitForSeconds(GetAttackDelay());

        for (int i = 0; i < burstAmount; i++)
        {
            projectile = Shoot(projectileSine.gameObject, target);
            audioManager.Play("Boss_Shoot");
            //Rotates the projectile to face the player
            projectile.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, target - projectile.transform.position), Vector3.forward);
            yield return new WaitForSeconds(firerate);
        }
    }

    public IEnumerator ShootSpin()
    {
        controller.bossAnimator.SetTrigger("Shoot_Spin");
        yield return new WaitForSeconds(GetAttackDelay());
        Instantiate(projectileSpin, transform.position, transform.rotation);
        audioManager.Play("Boss5_Spin", 0.7f, 1f);
        yield break;
    }

    //Shoots Desp1 (Fast red projectile)
    public IEnumerator ShootDesp1()
    {
        GameObject projectile;

        int randomSign = RandomSign();

        for (int i = 0; i < despShotAmount; i++) {
            projectile = Shoot(projectileDesp1.gameObject, Vector3.zero);
            audioManager.Play("Boss5_Desp1", 0.8f, 1f);
            audioManager.Play("Boss3_Laser", 0.7f, 1.5f);

            //Ramdomly decides to reverse the swing pattern of the projectile
            projectile.GetComponent<Projectile_Sine>().waveSpeed *= randomSign;
            yield return new WaitForSeconds(despFireRate);
        }
    }

    //Shoots Desp2 (Slow green projectile)
    public IEnumerator ShootDesp2()
    {
        GameObject projectile;
        projectile = Shoot(projectileDesp2.gameObject, Vector3.zero);
        audioManager.Play("Boss_Shoot", 0.9f, 0.5f);
        audioManager.Play("Boss3_Laser", 0.7f, 0.5f);

        float rng = Random.Range(-despVariablity, despVariablity);
        projectile.GetComponent<Projectile_Sine>().moveSpeed += rng;
        projectile.GetComponent<Projectile_Sine>().waveSpeed += rng;

        //Ramdomly decides to reverse the swing pattern of the projectile
        projectile.GetComponent<Projectile_Sine>().waveSpeed *= RandomSign();

        yield break;
    }

    public IEnumerator Move()
    {
        yield return GetComponent<Boss5_Move>().MoveToRandomNode();
        yield break;
    }

    private GameObject Shoot(GameObject bullet, Vector3 target)
    {
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

    //Returns either a 1 or a -1
    public int RandomSign()
    {
        int rng = Random.Range(0, 2) * 2 - 1;
        return rng;
    }

    public float GetAttackDelay()
    {
        return attackDelay / controller.bossLevel;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
