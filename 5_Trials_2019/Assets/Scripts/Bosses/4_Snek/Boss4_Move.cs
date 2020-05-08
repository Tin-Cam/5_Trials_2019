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
        List<int> ints = FindPath(2, 5);
        ints = FindPath(6, 9);
        ints = FindPath(1, 11);
        ints = FindPath(0, 11);       
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



    private List<int> FindPathRec(int node, int end, IDictionary<PathNode, bool> exploredNodes)
    {      
        PathNode currentNode = pathNodes[node];
        exploredNodes[currentNode] = true;

        List<int> result = new List<int>();

        if(node == end)
        {
            Debug.Log("Found the end!");
            result.Add(node);
            return result;
        }

        int min = 99;

        foreach(PathNode nextNode in currentNode.linkedNodes)
        {
            if (exploredNodes[nextNode])
                break;

            Debug.Log("Exploring " + currentNode.nodeNumber);
            List<int> list = FindPathRec(currentNode.nodeNumber, end, exploredNodes);

            if (list.Count < min)
                result = list;

        }

        result.Add(node);

        return result;
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
