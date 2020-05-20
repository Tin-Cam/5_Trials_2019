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
        if (desperation)
            yield break;

        //Toggle desperation mode
        desperation = true;
        move.SetSpeed(10);

        Color color = new Color(0.5f, 0.5f, 1, 1);
        foreach(Transform bodyPart in head.body)
        {
            SpriteRenderer render = bodyPart.GetComponent<SpriteRenderer>();
            render.color = color;
        }

        yield return new WaitForSeconds(5);
        ExitDesperationMode();
    }

    private void ExitDesperationMode()
    {
        move.ResetSpeed();
        desperation = false;
        foreach (Transform bodyPart in head.body)
        {
            SpriteRenderer render = bodyPart.GetComponent<SpriteRenderer>();
            render.color = Color.white;
        }
    }

    public IEnumerator Shoot()
    {
        int counter = 0;

        //Cycles through each segment (except the last one) and attempts to shoot it
        while (counter < shootCycleCount)
        {
            for (int i = 1; i < head.body.Count; i++)
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
        ExitDesperationMode();
    }
}
