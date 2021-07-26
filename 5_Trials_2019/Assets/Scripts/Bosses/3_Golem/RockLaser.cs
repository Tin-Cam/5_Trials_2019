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
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        laserManager = GetComponent<LaserManager>();
        rig = GetComponent<Rigidbody2D>();
        audioManager = AudioManager.instance;
        audioManager.Play("Thump", 0.75f, 1.5f);

        //Changes (mirrors) the direction the laser moves
        if (isMirror)
        {
            startPosition = new Vector3(-startPosition.x, startPosition.y, startPosition.z);
            targetPosition = new Vector3(-targetPosition.x, targetPosition.y, targetPosition.z);
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
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
        float idleTime = 0.5f;
        if(FlagManager.instance.easyMode)
            idleTime *= 2;

        //Gets into position
        yield return MoveToPosition(startPosition, startSpeed);
        yield return new WaitForSeconds(idleTime);

        //Indicates Attack
        float y = 0;
        if(isMirror)
            y = 180;
        audioManager.Play("Boss3_Indicate", 0.75f, 1.5f);
        yield return laserManager.IndicateLaser(1, Quaternion.Euler(0, y, -90));

        //Shoots and starts to move
        audioManager.Play("Boss3_Laser", 0.75f, 1.5f);
        StartCoroutine(laserManager.ShootLaser(Quaternion.Euler(0, 0, -90)));
        yield return MoveToPosition(targetPosition, speedDivision);       
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "On_Screen")
            Destroy(this.gameObject);
    }
}
