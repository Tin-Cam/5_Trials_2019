using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;

    public bool lockX;
    public bool lockY;

    public Vector3 offset;
    private Vector3 velocity = Vector3.one;

    private void FixedUpdate()
    {
        if (lockX & lockY)
            return;

        Vector3 destination = target.position + offset;

        destination = LockedPosition(destination);

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, destination, ref velocity, Time.deltaTime * smoothSpeed);

        transform.position = smoothedPosition;
    }

    private Vector3 LockedPosition(Vector3 destination)
    {
        if (lockX)
        {
            destination.x = transform.position.x;
        }
        if (lockY)
        {
            destination.y = transform.position.y;
        }

        return destination;
    }

    public void ResetCamera()
    {
        transform.position = Vector3.zero + offset;
    }

    public void ResetCamera(Vector3 position)
    {
        transform.position = position + offset;
    }

}
