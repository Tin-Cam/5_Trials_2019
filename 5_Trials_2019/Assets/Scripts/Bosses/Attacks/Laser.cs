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

    public bool indicateAttack = true;
    public GameObject indicator;
    public float indicatorTime = 1;

    

    // Start is called before the first frame update
    void Start()
    {
        laser.transform.localScale = new Vector3(1, 0, 1);
        laser.SetActive(false);
        StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        yield return LaserIndicate();
        laser.SetActive(true);
        yield return LaserGain();
        yield return LaserHold();
        yield return LaserDiminish();
        Destroy(this.gameObject);
    }

    private IEnumerator LaserIndicate()
    {
        Debug.Log("Indicating");
        if(indicateAttack)
            yield return new WaitForSeconds(indicatorTime);
        Destroy(indicator.gameObject);
    }

    private IEnumerator LaserGain()
    {
        Debug.Log("Gaining");
        while(!IncreaseScale(gainSpeed, maxWidth))
        {
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator LaserHold()
    {
        Debug.Log("Holding");
        yield return new WaitForSeconds(holdTime);
    }

    private IEnumerator LaserDiminish()
    {
        Debug.Log("Diminishing");
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
