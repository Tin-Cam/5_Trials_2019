using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLaser : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float speedDivision;
    public bool isMirror = false;

    private readonly float startSpeed = 2;
    private LaserManager laserManager;
    private Rigidbody2D rig;
    private bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        laserManager = GetComponent<LaserManager>();
        rig = GetComponent<Rigidbody2D>();


        //Changes (mirrors) the direction the laser moves
        if (isMirror)
        {
            startPosition = new Vector3(-startPosition.x, startPosition.y, startPosition.z);
            targetPosition = new Vector3(-targetPosition.x, targetPosition.y, targetPosition.z);
        }

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
        yield return MoveToPosition(startPosition, startSpeed);//Gets into position
        yield return new WaitForSeconds(1);

        yield return laserManager.IndicateLaser(1, Quaternion.Euler(0, 0, -90));//Indicates Attack

        StartCoroutine(laserManager.ShootLaser(Quaternion.Euler(0, 0, -90)));//Shoots and starts to move
        yield return MoveToPosition(targetPosition, speedDivision);

        Destroy(this.gameObject);
    }
}
