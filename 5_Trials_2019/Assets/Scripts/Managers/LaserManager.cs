using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public Laser_Indicator indicatorPrefab;
    public Laser laserPrefab;

    private Laser_Indicator currentIndicator;
    private Laser currentLaser;

    public IEnumerator IndicateLaser(float time, Quaternion angle)
    {
        currentIndicator = Instantiate(indicatorPrefab, transform.position, angle);
        currentIndicator.transform.parent = gameObject.transform;
        yield return currentIndicator.Indicate(time);
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
        currentLaser  = Instantiate(laserPrefab, transform.position, angle);
        currentLaser.transform.parent = gameObject.transform;
        return currentLaser;
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

    public void RemoveExcess()
    {
        //Remove Rock Laser
        try
        {
            Destroy(currentIndicator.gameObject);

        }
        catch (MissingReferenceException)
        {
            Debug.Log("MissingReferenceException for Indicator");
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("NullReferenceException for Indicator");
        }

        //Remove Spreadshot
        try
        {
            Destroy(currentLaser.gameObject);
        }
        catch (MissingReferenceException)
        {
            Debug.Log("MissingReferenceException for Laser");
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("NullReferenceException for Laser");
        }
    }
}
