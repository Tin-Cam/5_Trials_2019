using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Move : _MoveBase
{
    public float moveSpeed;

    public Transform[] InnerNodes;
    public Transform[] OuterNodes;

    private Boss6_Controller controller;

    public void Init()
    {
        controller = GetComponent<Boss6_Controller>();
        StartCoroutine(TestingLoop());
    }

    public IEnumerator TestingLoop()
    {
        yield return SlideIntoInner();

        StartCoroutine(TestingLoop());
    }

    public IEnumerator SlideIntoInner()
    {
        //Appear outside arena
        int rng = Random.Range(0, 4);
        GoToOuterPosition(rng);

        //Move to an inner node
        rng = Random.Range(1, 5);
        yield return MoveToPosition(InnerNodes[rng].position);
        yield return new WaitForSeconds(1);
    }

    public void Teleport()
    {
        //Play animation
        int rng = Random.Range(0, 4);
        GoToInnerPosition(rng);
    }

    public void GoToInnerPosition(int node)
    {
        Transform transform = InnerNodes[node];
        ChangePosition(transform);
    }

    public void GoToOuterPosition(int node)
    {
        Transform transform = OuterNodes[node];
        ChangePosition(transform);
    }

    private void ChangePosition(Transform node)
    {
        transform.position = node.position;
    }

    public IEnumerator MoveToPosition(Vector3 target)
    {
        bool isMoving = true;
        float speed = moveSpeed * controller.bossLevel;

        while (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);

            //Set position to node when boss is "close enough"
            if (Vector3.Distance(transform.position, target) < 0.05)
            {
                transform.position = target;
                isMoving = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
