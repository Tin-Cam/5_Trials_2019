using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{

    public Vector3 targetPosition;
    public float time;
    public float speed;

    public bool moveOnStart;
    

    // Start is called before the first frame update
    void Start()
    {
        if(moveOnStart)
            StartMove();
    }

    public void StartMove(){
        StartCoroutine(Move());
    }

    private IEnumerator Move(){
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            yield return new WaitForFixedUpdate();
        }
    }

}
