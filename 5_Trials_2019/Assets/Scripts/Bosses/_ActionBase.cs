using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _ActionBase : MonoBehaviour
{
    public List<string> actionList = new List<string>();

    public bool isActing;

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

    abstract public void DefaultState();
}
