using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public bool isShaking;
    public float intensity;
    public float speed;

    private Vector3 origin;
    private Vector3 position;
    private float t;

    void Start(){
        origin = transform.position;
    }

    private void FixedUpdate()
    {
        if(!isShaking)
            return;

        t += Time.deltaTime;

        float x = Mathf.Sin(t * speed) * intensity;
        x += origin.x;

        Vector3 position = new Vector3(x, origin.y, origin.z);
        transform.position = position;
    }

    public void StartShake(){
        isShaking = true;
        t = 0;
        origin = transform.position;
    }

    public void StopShaking(){
        isShaking = false;
    }

    public void SetShakeSpeed(float newSpeed){
        speed = newSpeed;
    }
}
