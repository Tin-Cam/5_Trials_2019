using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Indicator : MonoBehaviour
{
    public float indicatorTime = 1;

    public IEnumerator Indicate()
    {
        yield return new WaitForSeconds(indicatorTime);
        Destroy(this);
    }

}
