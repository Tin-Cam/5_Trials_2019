using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Move : _MoveBase
{
    public PathNode[] pathNodes;
    private SnakeMovement head;

    public int moveCount = 0;

    private float defaultSpeed;

    private int[] endNodes = {0, 1, 2, 5, 6, 9, 10, 11};
    private int[] middleNodes = {3, 4, 8, 7};


    public void Init(SnakeMovement head)
    {
        this.head = head;
        defaultSpeed = head.speed;
    }

    public IEnumerator StartToEnd()
    {
        int start = endNodes[Random.Range(0, endNodes.Length)];
        int end = endNodes[Random.Range(0, endNodes.Length)];

        //Ensures start and end aren't the same node
        while(start == end)
            end = endNodes[Random.Range(0, endNodes.Length)];

        int[] path = FindPath(start, end).ToArray();

        yield return FollowPath(path);
    }

    public IEnumerator StartToMiddleToEnd()
    {
        int start = endNodes[Random.Range(0, endNodes.Length)];
        int middle = middleNodes[Random.Range(0, middleNodes.Length)];
        int end = endNodes[Random.Range(0, endNodes.Length)];


        //Ensures start and end aren't the same node
        while (start == end)
            end = endNodes[Random.Range(0, endNodes.Length)];

        int[] path1 = FindPath(start, middle).ToArray();
        int[] path2 = FindPath(middle, end).ToArray();

        int[] fullPath = path1.Concat(path2).ToArray();

        yield return FollowPath(fullPath);
    }

    public IEnumerator CircleMiddle()
    {
        int start = endNodes[Random.Range(0, endNodes.Length)];       
        int end = endNodes[Random.Range(0, endNodes.Length)];

        int circleStart = Random.Range(0, middleNodes.Length);


        //Ensures start and end aren't the same node
        while (start == end)
            end = endNodes[Random.Range(0, endNodes.Length)];

        int[] path2 = CirclePath(circleStart, 3);

        int[] path1 = FindPath(start, path2[0]).ToArray();       
        int[] path3 = FindPath(path2[path2.Length - 1], end).ToArray();

        int[] fullPath = path1.Concat(path2).ToArray();
        fullPath = fullPath.Concat(path3).ToArray();

        yield return FollowPath(fullPath);
    }

    private int[] CirclePath(int node, int rotations)
    {
        List<int> path = new List<int>();

        for(int i = 0; i < rotations * 4; i++)
        {
            path.Add(middleNodes[node]);
            node++;
            if (node >= middleNodes.Length)
                node = 0;
        }
        return path.ToArray();
    }


    public IEnumerator FollowPath(int[] path)
    {
        moveCount = 0;
        Vector3 startPos = pathNodes[path[0]].transform.position;
        head.Teleport(startPos);

        foreach(int node in path)
        {
            SetDestination(node);
            while (!head.atDestination)
            {
                yield return new WaitForFixedUpdate();
            }
            moveCount++;
        }
        moveCount = 0;
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


    //Changes speed of the body
    public void SetSpeed(float speed)
    {
        head.speed = speed;
    }
    //Changes speed of the body and changes the default speed
    public void HardSetSpeed(float speed)
    {
        head.speed = speed;
        defaultSpeed = speed;
    }
    //Resets speed back to default
    public void ResetSpeed()
    {
        head.speed = defaultSpeed;
    }
    //Returns Default Speed
    public float GetDefaultSpeed()
    {
        return defaultSpeed;
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
