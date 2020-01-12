using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Actions : _ActionBase
{
    //public Laser laser;

    private Boss3_Controller controller;
    private LaserManager laserManager;
    private GameObject player;

    public void Init()
    {
        controller = GetComponent<Boss3_Controller>();
        laserManager = GetComponent<LaserManager>();
        player = controller.player;

        StartCoroutine(ShootLaser());
    }

    //ACTIONS ---------------------

    public IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(1);
        ShootLaser(player.transform.position);
        yield return new WaitForSeconds(2);

        ShootLaser(player.transform.position, BigLaser());

        yield return new WaitForSeconds(2);
        ShootLaser(player.transform.position);
    }


    public void ShootLaser(Vector3 target)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        laserManager.CreateLaser(targetAngle);
    }

    public void ShootLaser(Vector3 target, Laser laser)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        laserManager.CreateLaser(targetAngle, laser);
    }

    public Laser BigLaser()
    {
        Laser laser = new Laser
        {
            gainSpeed = (float)0.1,
            diminishSpeed = (float)0.1,
            holdTime = 5,
            maxWidth = 10,

            indicateAttack = true,
            indicatorTime = 3
        };

        return laser;
    }

    public override void DefaultState()
    {
        
    }
}
