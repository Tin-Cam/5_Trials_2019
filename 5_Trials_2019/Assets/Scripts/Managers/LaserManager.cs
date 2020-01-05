using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public Laser laserPrefab;

    public Laser GetLaser()
    {
        return laserPrefab;
    }

    public Laser CreateLaser(float gainSpeed, float dimisnishSpeed, float holdTime)
    {
        Laser laser = CreateLaser();

        laser.gainSpeed = gainSpeed;
        laser.diminishSpeed = dimisnishSpeed;
        laser.holdTime = holdTime;

        return laser;
    }

    private Laser CreateLaser()
    {
        Laser laser = new Laser();
        laser.laser = laserPrefab.laser;
        laser.indicator = laserPrefab.indicator;
        return laser;
    }
}
