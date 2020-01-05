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

        Laser laser = laserManager.CreateLaser((float) 0.01, (float) 0.1, (float) 40);
        ShootLaser(player.transform.position, laser);


        yield return new WaitForSeconds(2);
        ShootLaser(player.transform.position);
    }

    public void ShootLaser(Vector3 target)
    {
        Laser laser = laserManager.GetLaser();

        ShootLaser(target, laser);
    }

    public void ShootLaser(Vector3 target, Laser laser)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(laserManager.GetLaser(), transform.position, targetAngle);
    }

    public override void DefaultState()
    {
        
    }
}
