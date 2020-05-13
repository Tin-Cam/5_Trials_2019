  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Action : _ActionBase
{
    public bool desperation = false;
    private SnakeMovement head;

    private Boss4_Move move;
    
    public void Init(SnakeMovement head)
    {
        move = GetComponent<Boss4_Move>();
        this.head = head;
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
        //Toggle desperation mode
        desperation = !desperation;

        if (desperation)
            move.SetSpeed(20);
        else
            move.SetSpeed(5);

        yield break;
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
