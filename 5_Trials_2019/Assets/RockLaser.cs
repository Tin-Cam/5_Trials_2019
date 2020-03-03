using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLaser : MonoBehaviour
{
    private LaserManager laserManager;

    // Start is called before the first frame update
    void Start()
    {
        laserManager = GetComponent<LaserManager>();
        StartCoroutine(FireSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FireSequence()
    {
        yield return laserManager.ShootLaser(Quaternion.Euler(0, 0, -90));
    }
}
