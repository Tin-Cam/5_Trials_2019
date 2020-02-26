using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public Laser_Indicator indicatorPrefab;
    public Laser laserPrefab;


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

    public IEnumerator ShootLaser(Quaternion angle, Laser laserRef)
    {
        Laser laser = CreateLaser(angle, laserRef);
        yield return laser.FireLaser();
        yield break;
    }

    public Laser CreateLaser(Quaternion angle)
    {
        Laser laser  = Instantiate(laserPrefab, transform.position, angle);
        return laser;
    }

    //Copies the values of a laser object to a prefab instance
    public Laser CreateLaser(Quaternion angle, Laser laserRef)
    {
        Laser laser = CreateLaser(angle);

        laser.gainSpeed = laserRef.gainSpeed;
        laser.diminishSpeed = laserRef.diminishSpeed;
        laser.holdTime = laserRef.holdTime;
        laser.maxWidth = laserRef.maxWidth;

        return laser;
    }
}
