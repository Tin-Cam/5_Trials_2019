using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Move : _MoveBase
{
    public float moveSpeed;

    public bool isMoving;

    public Transform[] MoveNodes;
    private int currentNode;

    public void Init()
    {

    }

    public IEnumerator MoveToRandomNode()
    {
        int rng = Random.Range(1, MoveNodes.Length);
        while(rng == currentNode)
            rng = Random.Range(1, MoveNodes.Length);

        yield return MoveToNodeCO(rng);
    }

    public IEnumerator MoveToNodeCO(int node)
    {
        isMoving = true;
        Vector3 targetNode = MoveNodes[node].position;

        while (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetNode, Time.deltaTime * moveSpeed);

            //Set position to node when boss is "close enough". (Speeds up move time)
            if (Vector3.Distance(transform.position, targetNode) < 0.05)
            {
                transform.position = targetNode;
                isMoving = false;
            }

            yield return new WaitForFixedUpdate();
        }
        currentNode = node;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
