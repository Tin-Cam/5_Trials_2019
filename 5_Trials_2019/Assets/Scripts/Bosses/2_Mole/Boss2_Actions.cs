using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_Actions : _ActionBase
{
    public float idleTime;

    private Boss2_Move move;

    public void Init()
    {
        move = GetComponent<Boss2_Move>();

        actionList.Add("Idle");
        actionList.Add("MoveRandom");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ACTIONS ---------------------------------

    //Action 0 - Idle
    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);
    }

    //Action 1 - Move randomly across the room
    private IEnumerator MoveRandom()
    {
        move.MovePosition(3);
        Debug.Log("Move Done");
        yield return new WaitForEndOfFrame();
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
