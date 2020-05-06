using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Move : _MoveBase
{
    public PathNode[] path;
    public SegmentedTileMove[] bodySegments;
    private SegmentedTileMove head;

    public int nodeCount = 0;
    private int[] track = { 4, 8, 5, 10, 9, 5, 4 };

    public void Init()
    {
        head = bodySegments[0];

        SetDestination(8);
    }

    void FixedUpdate()
    {
        if (head.atDestination)
        {
            SetDestination(track[nodeCount]);
            nodeCount++;
        }
    }




    public void SetDestination(int node)
    {
        head.SetDestination(path[node - 1].transform.position);
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }


}
