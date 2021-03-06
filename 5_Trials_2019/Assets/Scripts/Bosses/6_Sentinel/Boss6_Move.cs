﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Move : _MoveBase
{
    public float moveSpeed;
    public float teleportTime;
    [HideInInspector]
    public bool isCircling = false;

    public Transform[] InnerNodes;
    public Transform[] OuterNodes;

    private Boss6_Controller controller;
    private AnimatorScripts animatorScripts;
    private AudioManager audioManager;

    public void Init()
    {
        controller = GetComponent<Boss6_Controller>();
        animatorScripts = GetComponent<AnimatorScripts>();
        audioManager = AudioManager.instance;
    }

    //TELEPORTING --------------------------

    public IEnumerator Teleport(Vector3 position)
    {
        //Play animation
        animatorScripts.PlayAnimation("Boss_6_Idle", 0);
        yield return EnterTeleport(new Vector3(0, 20, 0));

        yield return new WaitForSeconds(teleportTime * GetLevelFraction());

        yield return ExitTeleport(position);
    }

    public IEnumerator EnterTeleport(Vector3 position)
    {
        audioManager.Play("Teleport", 0.75f, 1);
        yield return animatorScripts.PlayWholeAnimation("Boss_6_Teleport", 2);
        ChangePosition(position);
    }

    public IEnumerator ExitTeleport(Vector3 position)
    {
        ChangePosition(position);
        audioManager.Play("Teleport", 0.75f, 0.75f);
        yield return animatorScripts.PlayWholeAnimation("Boss_6_Teleport_Appear", 2);
        animatorScripts.PlayAnimation("Boss_6_Teleport_Normal", 2);
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
        //Pick an inner node
        int rng = Random.Range(1, 5);
        yield return EnterToInner(rng);
    }

    public IEnumerator EnterToInner(int node)
    {
        //Appear outside arena
        int rng = Random.Range(0, 4);
        GoToOuterPosition(rng);
        //Move to node
        audioManager.Play("Whoosh", 0.75f, 1);        
        yield return ZipToPosition(InnerNodes[node].position);
        yield return new WaitForSeconds(1 * GetLevelFraction());
    }

    public IEnumerator Exit()
    {
        //Move to an outer node
        animatorScripts.PlayAnimation("Boss_6_Idle", 0);
        int rng = Random.Range(0, 4);
        audioManager.Play("Whoosh", 0.75f, 0.75f);
        yield return ZipToPosition(OuterNodes[rng].position);
        yield return new WaitForSeconds(1 * GetLevelFraction());
    }

    //Moves to a position with a rubberband effect applied to speed
    public IEnumerator ZipToPosition(Vector3 target)
    {
        bool isMoving = true;
        
        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * RubberBandSpeed(target));
            isMoving = !CheckIfClose(target);
            yield return new WaitForFixedUpdate();
        }
    }

    //Smoothly moves to a position (No rubberband effect)
    public IEnumerator GlideToPosition(Vector3 target)
    {
        bool isMoving = true;
        float speed = moveSpeed * controller.bossLevel;

        audioManager.Play("Whoosh", 0.75f, 0.25f);
        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
            isMoving = !CheckIfClose(target);
            yield return new WaitForFixedUpdate();
        }
    }

    //Set position to node when boss is "close enough"
    private bool CheckIfClose(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < 0.01)
        {
            transform.position = target;
            return true;
        }
        return false;
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
    public IEnumerator EnterToCircle()
    {
        audioManager.Play("Whoosh", 0.75f, 1);
        yield return ZipToPosition(CirclePos(Vector2.zero, 5f, 0));
    }

    public IEnumerator CircleCentre()
    {
        float t = 0;
        float speed = moveSpeed * 0.1f;

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

    //MISC --------------------------

    public IEnumerator MoveToDesperation()
    {
        Vector2 targetPos = InnerNodes[0].position;

        float t = -15;
        float amplitude = 4;
        float speed = moveSpeed * controller.bossLevel * 5f;

        while (t <= 0)
        {
            float x = Mathf.Sin(Mathf.PI * t * 0.4f) * amplitude + targetPos.x;
            float y = t + targetPos.y;

            transform.position = new Vector2(x, y);

            t += speed * Time.deltaTime * 0.2f;
            yield return new WaitForFixedUpdate();
        }
        transform.position = targetPos;       
    }

    private float GetLevelFraction()
    {
        return 1 / controller.bossLevel;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
