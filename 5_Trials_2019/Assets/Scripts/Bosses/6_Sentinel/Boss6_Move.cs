using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Move : _MoveBase
{
    public Animator animator;

    public float moveSpeed;
    public float teleportTime;
    [HideInInspector]
    public bool isCircling = false;

    public Transform[] InnerNodes;
    public Transform[] OuterNodes;

    private Boss6_Controller controller;

    public void Init()
    {
        controller = GetComponent<Boss6_Controller>();
    }

    //TELEPORTING --------------------------

    public IEnumerator Teleport(Vector3 position)
    {
        //Play animation
        yield return PlayAnimation("Boss_6_Teleport", 2);
        yield return new WaitForSeconds(teleportTime);


        ChangePosition(position);
        yield return PlayAnimation("Boss_6_Teleport_Appear", 2);
    }

    public IEnumerator RandomTeleport()
    {
        int rng = Random.Range(1, 5);
        Transform transform = InnerNodes[rng];
        yield return Teleport(transform.position);
    }

    public void GoToInnerPosition(int node)
    {
        Transform transform = InnerNodes[node];
        ChangePosition(transform.position);
    }

    public void GoToOuterPosition(int node)
    {
        Transform transform = OuterNodes[node];
        ChangePosition(transform.position);
    }

    private void ChangePosition(Vector3 position)
    {
        transform.position = position;
    }

    //MOVE TO POSITION --------------------------

    public IEnumerator EnterToInner()
    {
        //Appear outside arena
        int rng = Random.Range(0, 4);
        GoToOuterPosition(rng);

        //Move to an inner node
        rng = Random.Range(1, 5);
        yield return MoveToPosition(InnerNodes[rng].position);
        yield return new WaitForSeconds(1);
    }

    public IEnumerator Exit()
    {
        //Move to an outer node
        int rng = Random.Range(0, 4);
        yield return MoveToPosition(OuterNodes[rng].position);
        yield return new WaitForSeconds(1);
    }

    public IEnumerator MoveToPosition(Vector3 target)
    {
        bool isMoving = true;

        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * RubberBandSpeed(target));

            //Set position to node when boss is "close enough"
            if (Vector3.Distance(transform.position, target) < 0.01)
            {
                transform.position = target;
                isMoving = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //Speeds up or slows down boss depending on how far away it's from it's destination
    private float RubberBandSpeed(Vector3 target)
    {
        float defaultSpeed = moveSpeed * controller.bossLevel;
        float result = defaultSpeed * Vector3.Distance(transform.position, target);

        //Caps speed at either half or double the original move speed
        if (result < defaultSpeed / 2)
            result = defaultSpeed / 2;

        if (result > defaultSpeed * 10)
            result = defaultSpeed * 10;

        return result;
    }

    //CIRCLING --------------------------
    public IEnumerator CircleCentre()
    {
        float t = 0;
        float speed = moveSpeed * controller.bossLevel * 0.1f;

        //yield return MoveToPosition(MoveInCircle(Vector2.zero, 5f, t));

        isCircling = true;
        while (isCircling && t < (5 * 2)){
            transform.position = CirclePos(Vector2.zero, 5f, t);
            t += speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isCircling = false;
        //yield return Exit();
    }
    
    //Calculates the point of a circle
    public Vector2 CirclePos(Vector2 origin, float radius, float t)
    {
        float x = Mathf.Sin(Mathf.PI * t) * radius + origin.x;
        float y = Mathf.Cos(Mathf.PI * t) * radius + origin.y;

        Vector2 result = new Vector2(x, y);

        return result;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }

    //Finishes when an animation stops playing
    public IEnumerator PlayAnimation(string animation, int layer)
    {
        animator.Play(animation, layer);
        yield return new WaitForEndOfFrame();
        Debug.Log("Waiting for " + animation);
        while (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
