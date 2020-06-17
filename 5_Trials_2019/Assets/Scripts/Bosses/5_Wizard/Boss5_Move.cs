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
        float length = Vector3.Distance(transform.position, targetNode);

        float t = 0;

        while (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetNode, t / length);

            t += Time.deltaTime * moveSpeed;

            if (transform.position == targetNode)
                isMoving = false;

            yield return new WaitForFixedUpdate();
        }
        currentNode = node;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
