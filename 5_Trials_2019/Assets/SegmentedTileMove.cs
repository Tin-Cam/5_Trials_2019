using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentedTileMove : MonoBehaviour
{
    public float speed;

    public bool isHead;
    public bool isEnd;

    public SegmentedTileMove childSegment;
    //public SegmentedTileMove parentSegment;

    private Vector3 nextTile;
    public Vector3 destinationTile;
    public Vector3 moveOffset;

    private bool wasLastMoveX;

    void Awake()
    {
        nextTile = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (!isEnd)
            childSegment.nextTile = transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextTile, speed * Time.deltaTime);

        if (transform.position == nextTile)
        {
            if(isHead)
                CalculateNextTile();
        }
    }

    private void CalculateNextTile()
    {     
        float x = transform.position.x;
        float y = transform.position.y;

        //Check x

        if (!wasLastMoveX) {
            x = NudgeNumber(transform.position.x, destinationTile.x);
            wasLastMoveX = true;
        }

        if (x == transform.position.x)
        {
            y = NudgeNumber(transform.position.y, destinationTile.y);
            wasLastMoveX = false;
        }


        Vector3 tile = new Vector3(x, y, 0);
        tile += moveOffset;
        UpdateNextTiles(tile);
    }

    private float NudgeNumber(float number, float target)
    {
        float amount = 1;

        //Add amount if less than target
        if (number < target)
            number += amount;

        //Subtract amount if higher than target
        else if (number > target)
            number -= amount;

        number = Mathf.RoundToInt(number);

        return number;
    }

    public void UpdateNextTiles(Vector3 tile)
    {
        nextTile = tile;
        if (!isEnd)
            childSegment.UpdateNextTiles(transform.position);
    }
}
