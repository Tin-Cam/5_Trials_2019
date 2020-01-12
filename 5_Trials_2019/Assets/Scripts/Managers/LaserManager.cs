using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public Laser laserPrefab;

    //Copies the values of a laser object to a prefab instance
    public Laser CreateLaser(Quaternion angle, Laser laserClass)
    {
        Laser laser = CreateLaser(angle);

        laser.gainSpeed = laserClass.gainSpeed;
        laser.diminishSpeed = laserClass.diminishSpeed;
        laser.holdTime = laserClass.holdTime;
        laser.maxWidth = laserClass.maxWidth;

        laser.indicateAttack = laserClass.indicateAttack;
        laser.indicatorTime = laserClass.indicatorTime;

        return laser;
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
