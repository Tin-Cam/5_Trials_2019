using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Move : _MoveBase
{
    public PathNode[] pathNodes;
    private SnakeMovement head;

    public int nodeCount = 0;

    private int[] endNodes = {0, 1, 2, 5, 6, 9, 10, 11};
    private int[] middleNodes = {3, 4, 7, 8};


    public void Init(SnakeMovement head)
    {
        this.head = head;
    }

    public IEnumerator StartToEnd()
    {
        int start = endNodes[Random.Range(0, endNodes.Length)];
        int end = endNodes[Random.Range(0, endNodes.Length)];

        Debug.Log("Start: " + start + " End: " + end);

        //Ensures start and end aren't the same node
        while(start == end)
            end = endNodes[Random.Range(0, endNodes.Length)];

        int[] path = FindPath(start, end).ToArray();

        yield return FollowPath(path);
    }



    public IEnumerator FollowPath(int[] path)
    {
        Vector3 startPos = pathNodes[path[0]].transform.position;
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

    //PATHFINDING ------------------------------------


    //Finds the path between 2 points (Note: Works from the end to the start node)
    public List<int> FindPath(int start, int end)
    {
        Dictionary<int, int> nodeParents = new Dictionary<int, int>();
        Queue<int> queue = new Queue<int>();
        HashSet<int> exploredNodes = new HashSet<int>();

        queue.Enqueue(end);

        

        while (queue.Count != 0)
        {
            int currentNode = queue.Dequeue();
            if (currentNode == start)
                break;

            IList<int> linkedNodes = GetLinkedNodes(currentNode);

            foreach (int node in linkedNodes)
            {
                if (!exploredNodes.Contains(node))
                {

                    exploredNodes.Add(node);

                    nodeParents.Add(node, currentNode);

                    queue.Enqueue(node);
                }
            }
        }
        Debug.Log(nodeParents);
        List<int> result = new List<int>();

        int writeNode = start;

        while (writeNode != end)
        {
            result.Add(writeNode);

            writeNode = nodeParents[writeNode];
        }
        result.Add(writeNode);

        return result;
    }


    public IList<int> GetLinkedNodes(int node)
    {
        IList<int> result = new List<int>();
        PathNode pathNode = pathNodes[node];

        foreach (PathNode nextNode in pathNode.linkedNodes)
        {
            result.Add(nextNode.nodeNumber);
        }

        return result;
    }






    public void SetSpeed(float speed)
    {
        head.speed = speed;
    }



    public void SetDestination(int node)
    {
        head.SetDestination(pathNodes[node].transform.position);
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }


}
