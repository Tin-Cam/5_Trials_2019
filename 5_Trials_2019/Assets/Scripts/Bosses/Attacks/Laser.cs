using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laser;
    public float gainSpeed;    
    public float diminishSpeed;
    public float holdTime;
    public float maxWidth;
    

    // Start is called before the first frame update
    void Start()
    {
        laser.transform.localScale = new Vector3(1, 0, 1);
        laser.SetActive(false);
        StartCoroutine(FireLaser());
    }

    //Works through the steps of firing a laser
    private IEnumerator FireLaser()
    {
        laser.SetActive(true);
        yield return LaserGain();
        yield return LaserHold();
        yield return LaserDiminish();
        Destroy(this.gameObject);
    }

    private IEnumerator LaserGain()
    {
        while(!IncreaseScale(gainSpeed, maxWidth))
        {
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator LaserHold()
    {
        yield return new WaitForSeconds(holdTime);
    }

    private IEnumerator LaserDiminish()
    {
        while (!ReduceScale(diminishSpeed, 0))
        {
            yield return new WaitForFixedUpdate();
        }      
    }

    //Algorithms that handle changing the laser's size
    private bool IncreaseScale(float y, float targetY)
    {
        AppendScale(y);

        if (laser.transform.localScale.y >= targetY)
            return true;
        return false;
    }

    private bool ReduceScale(float y, float targetY)
    {
        AppendScale(-y);

        if (laser.transform.localScale.y <= targetY)
            return true;
        return false;
    }

    private void AppendScale(float y)
    {
        Vector3 scale = new Vector3(0, y, 0);
        laser.transform.localScale += scale;
    }
}
