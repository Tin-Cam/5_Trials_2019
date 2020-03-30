using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float timer;
    private float timerCount = 0;

    // Update is called once per frame
    void Update()
    {
        
        timerCount += Time.deltaTime;

        if (timerCount >= timer)
            Destroy(this.gameObject);
    }
}