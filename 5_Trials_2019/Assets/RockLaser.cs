using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLaser : MonoBehaviour
{
    public Vector3 startPosition;
    public float startSpeed;

    private LaserManager laserManager;
    private Rigidbody2D rig;
    private bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        laserManager = GetComponent<LaserManager>();
        rig = GetComponent<Rigidbody2D>();
        StartCoroutine(FireSequence());
    }

    private IEnumerator MoveToPosition()
    {
        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, startSpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FireSequence()
    {
        yield return MoveToPosition();
        yield return laserManager.IndicateLaser(1, Quaternion.Euler(0, 0, -90));
        yield return laserManager.ShootLaser(Quaternion.Euler(0, 0, -90));
    }

    void FixedUpdate()
    {
        if (!isShooting)
            return;

        Vector2 movement;

        //rig.MovePosition(movement * new Vector2(Time.fixedDeltaTime, 1));
    }
}
