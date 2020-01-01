using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float gainSpeed;    
    public float diminishSpeed;
    public float holdTime;

    public float maxWidth;

    private int state;
    // 0: Gaining
    // 1: Holding
    // 2: Diminishing

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1, 0, 1);
        StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        yield return LaserGain();
        yield return LaserHold();
        yield return LaserDiminish();
        Destroy(this.gameObject);
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
        yield return new WaitForSeconds(2);
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

        if (transform.localScale.y >= targetY)
            return true;
        return false;
    }

    private bool ReduceScale(float y, float targetY)
    {
        AppendScale(-y);

        if (transform.localScale.y <= targetY)
            return true;
        return false;
    }

    private void AppendScale(float y)
    {
        Vector3 scale = new Vector3(0, y, 0);
        transform.localScale += scale;
    }
}
