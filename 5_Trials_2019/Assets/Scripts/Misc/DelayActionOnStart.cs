using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayActionOnStart : MonoBehaviour
{
    public float delayTime;
    public UnityEvent meths;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimerCO());
    }

    private IEnumerator TimerCO(){
        while(delayTime > 0){
            delayTime -= Time.deltaTime;       
            yield return new WaitForEndOfFrame();
        }
        //Executes meths
        meths.Invoke();
    }

}
