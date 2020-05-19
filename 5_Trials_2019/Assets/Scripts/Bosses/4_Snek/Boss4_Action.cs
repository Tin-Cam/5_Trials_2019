  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Action : _ActionBase
{
    public bool desperation = false;

    public int shootCycleCount;

    public Projectile bomb;

    private SnakeMovement head;

    private Boss4_Controller controller;
    private Boss4_Move move;
    private GameObject player;

    public void Init(SnakeMovement head)
    {
        controller = GetComponent<Boss4_Controller>();
        move = GetComponent<Boss4_Move>();

        this.head = head;
        player = controller.player;

        actionList.Add("DesperationMode");
        actionList.Add("Shoot");
        actionList.Add("LayBomb");
    }
    
    public IEnumerator DesperationMode()
    {
        //Toggle desperation mode
        desperation = !desperation;

        if (desperation)
            move.SetSpeed(10);
        else
            move.SetSpeed(5);

        yield break;
    }

    public IEnumerator Shoot()
    {
        int counter = 0;

        //Cycles through each segment (except the last one) and attempts to shoot it
        while (counter < shootCycleCount)
        {
            for(int i = 1; i < head.body.Count; i++)
            {
                Segment segment = head.body[i - 1].GetComponent<Segment>();
                if (!segment.IsDestroyed())
                {
                    segment.Shoot(player.transform.position);
                    counter++;
                    yield return new WaitForSeconds(0.1f);
                }             
            }
        } 
    }

    public IEnumerator LayBomb()
    {
        Segment segment = head.body[head.body.Count - 1].GetComponent<Segment>();
        segment.Shoot(player.transform.position);

        yield break;
    }

    public bool CheckSegmentBounds(int segmentNO)
    {
        Segment segment = head.body[segmentNO].GetComponent<Segment>();
        return segment.CheckShootBounds();
    }


    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
