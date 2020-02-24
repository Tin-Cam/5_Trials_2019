using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public Laser_Indicator indicatorPrefab;
    public Laser laserPrefab;

    //Copies the values of a laser object to a prefab instance
    public Laser CreateLaser(Quaternion angle, Laser laserClass)
    {
        Laser laser = CreateLaser(angle);

        laser.gainSpeed = laserClass.gainSpeed;
        laser.diminishSpeed = laserClass.diminishSpeed;
        laser.holdTime = laserClass.holdTime;
        laser.maxWidth = laserClass.maxWidth;

        //laser.indicateAttack = laserClass.indicateAttack;
        //laser.indicatorTime = laserClass.indicatorTime;

        return laser;
    }

    public IEnumerator IndicateLaser(float time, Quaternion angle)
    {
        Laser_Indicator indicator = Instantiate(indicatorPrefab, transform.position, angle);
        yield return indicator.Indicate(time);
    }

    public IEnumerator ShootLaser(Quaternion angle)
    {
        Laser laser = CreateLaser(angle);
        yield return laser.FireLaser();
        yield break;
    }


    public Laser CreateLaser(Quaternion angle)
    {
        Laser laser  = Instantiate(laserPrefab, transform.position, angle);
        return laser;
    }

    public Laser CreateLaser(Quaternion angle, float gainSpeed, float dimisnishSpeed, float holdTime)
    {
        Laser laser = CreateLaser(angle);

        laser.gainSpeed = gainSpeed;
        laser.diminishSpeed = dimisnishSpeed;
        laser.holdTime = holdTime;

        return laser;
    }
}
