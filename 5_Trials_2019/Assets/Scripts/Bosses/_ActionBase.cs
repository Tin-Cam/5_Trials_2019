using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _ActionBase : MonoBehaviour
{
    public List<string> actionList = new List<string>();

    public void StartAction(int action)
    {
        if (action > actionList.Count)
            return;

        StartCoroutine(actionList[action]);
    }
}
