using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Move : _MoveBase
{
    public PathNode[] pathNodes;
    public SnakeMovement head;

    public int nodeCount = 0;

    private int[] track = { 1, 4, 8, 7, 9, 5, 4 };
    private int[] track2 = { 6, 5, 4, 8, 11 };

    public void Init()
    {
        
    }


    public IEnumerator FollowPath(int[] path)
    {
        Vector3 startPos = pathNodes[path[0] - 1].transform.position;
        head.Teleport(startPos);

        foreach(int node in path)
        {
            SetDestination(node);
            while (!head.atDestination)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }


    public void SetDestination(int node)
    {
        head.SetDestination(pathNodes[node - 1].transform.position);
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }


}
