using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _ActionBase : MonoBehaviour
{
    public List<string> actionList = new List<string>();

    public bool isActing;

    private Vector2Int last2Actions;

    //Makes the boss perform an action
    public void StartAction(int action)
    {
        if (action > actionList.Count)
            return;

        StartCoroutine(actionList[action]);
    }

    //Stops the boss from performing any action
    public void StopActing()
    {
        StopAllCoroutines();
        isActing = false;
    }

    //Ensures no action is used 3 times in a row
    public bool CheckLastAction(int action)
    {
        int actionSum = action * 2;
        int vecSum = last2Actions.x + last2Actions.y;

        if (actionSum == vecSum)
            return true;

        last2Actions.y = last2Actions.x;
        last2Actions.x = action;

        return false;
    }

    abstract public void DefaultState();
}
