using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{

    public List<Transform> body = new List<Transform>();

    public float minDistance = 0.25f;

    public float speed = 1;

    public Vector3 destination;
    public bool atDestination;

    private float distance;
    private Transform currentSegment;
    private Transform previousSegment;

    // Start is called before the first frame update
    void Start()
    {
        atDestination = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        if (transform.position == destination)
            atDestination = true;
    }

    private void Move()
    {
        //Moves the head
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        //Moves the body parts
        for (int i = 1; i < body.Count; i++)
        {
            currentSegment = body[i];
            previousSegment = body[i - 1];

            distance = Vector3.Distance(previousSegment.position, currentSegment.position);

            Vector3 newpos = previousSegment.position;

            newpos.z = body[0].position.z;

            float t = Time.deltaTime * distance / minDistance * speed;

            if (t > 0.5f)
                t = 0.5f;
            currentSegment.position = Vector3.Slerp(currentSegment.position, newpos, t);
            currentSegment.rotation = Quaternion.Slerp(currentSegment.rotation, previousSegment.rotation, t);
        }
    }

    public void SetDestination(Vector3 location)
    {
        destination = location;
        atDestination = false;
    }

    public void Teleport(Vector3 location)
    {
        foreach (Transform segment in body)
            segment.position = location;
    }
}
