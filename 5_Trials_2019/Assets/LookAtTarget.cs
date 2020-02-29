using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    public float speed;
    public bool isAiming = true;
    public Quaternion aimAngle;

    // Update is called once per frame
    void Update()
    {
        if (!isAiming)
            return;

        Quaternion targetAngle = Quaternion.LookRotation(target.transform.position - transform.position, transform.TransformDirection(Vector3.forward));

        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, speed * Time.deltaTime);

        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        aimAngle = transform.rotation * Quaternion.Euler(0, 0, -90);
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
