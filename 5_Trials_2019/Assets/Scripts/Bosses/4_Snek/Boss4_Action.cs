  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Action : _ActionBase
{
    public bool desperation = false;
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
        foreach(Transform bodyPart in head.body)
        {
            Segment segment = bodyPart.GetComponent<Segment>();
            segment.Shoot(player.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator LayBomb()
    {
        //Toggle desperation mode
        desperation = !desperation;

        if (desperation)
            move.SetSpeed(30);
        else
            move.SetSpeed(5);

        yield break;
    }




    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
