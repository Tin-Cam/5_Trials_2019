using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Indicator : MonoBehaviour
{
    public IEnumerator Indicate(float indicatorTime)
    {
        yield return new WaitForSeconds(indicatorTime);
        Destroy(this.gameObject);
    }
}
