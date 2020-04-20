using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsX : MonoBehaviour
{
    public float x;
    public float speed;

    void FixedUpdate()
    {
        Vector3 target = new Vector3(x, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, target, speed);
    }
}
