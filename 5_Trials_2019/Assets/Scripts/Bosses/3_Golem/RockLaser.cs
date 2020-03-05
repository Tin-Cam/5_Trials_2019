using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLaser : MonoBehaviour
{
    public Vector3 startPosition;
    public float speedDivision;

    private readonly float startSpeed = 2;
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

    private IEnumerator MoveToPosition(Vector3 target, float stepSpeed)
    {
        float step = 0;
        float rate = 1 / stepSpeed;

        while (transform.position != target)
        {
            step += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(transform.position, target, step);
            
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FireSequence()
    {
        Vector3 pos = new Vector3(-20, startPosition.y, startPosition.z);

        yield return MoveToPosition(startPosition, startSpeed);
        yield return laserManager.IndicateLaser(1, Quaternion.Euler(0, 0, -90));
        StartCoroutine(laserManager.ShootLaser(Quaternion.Euler(0, 0, -90)));
        yield return MoveToPosition(pos, speedDivision);
        Destroy(this.gameObject);
    }

}
