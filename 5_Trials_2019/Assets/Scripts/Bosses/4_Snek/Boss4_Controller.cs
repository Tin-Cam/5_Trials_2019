using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Controller : _BossBase
{
    private Boss4_Move move;
    private Boss4_Action action;

    public int minIdle;
    public int maxIdle;

    public SnakeMovement head;

    protected override void Init()
    {
        move = GetComponent<Boss4_Move>();
        action = GetComponent<Boss4_Action>();

        moveBase = move;
        actionBase = action;

        foreach(Transform bodyPart in head.body)
        {
            Segment segment = bodyPart.GetComponent<Segment>();
            segment.SetController(this);
            segment.SetAction(action);
        }


        move.Init(head);
        action.Init(head);

        StartCoroutine(Action());
    }

    public IEnumerator Action()
    {
        yield return move.StartToEnd();
        StartCoroutine(Action());
    }



    //Handles idleing
    public IEnumerator Idle()
    {
        //Wait Idle for some time
        float rngIdle = Random.Range(minIdle, (maxIdle + 1));
        float idleTimer = 0;

        while (idleTimer < rngIdle)
        {
            idleTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    //Not used
    public override void BossHurt()
    {
        
    }

    public void SegmentHurt()
    {
        TakeDamage(1);
    }

    public override void DefaultState()
    {
        //isHitable = true;
    }

    

    protected override void CheckHealth()
    { 

    }

    protected override void IncreasePhase()
    {
        throw new System.NotImplementedException();
    }



    protected override void StartDeath()
    {
        //Explosions for bodyparts
        for (int i = 1; i < head.body.Count; i++)
            Instantiate(deathExplosion, head.body[i].position, transform.rotation);
        //Explosion for head (AKA moving the controller to the head's position so that the explosion occurs at that positon instead <insert galaxy brain>)
        transform.position = head.transform.position;

        Die();
    }

    //REDUNDANT
    protected override void Act()
    {

    }
}
