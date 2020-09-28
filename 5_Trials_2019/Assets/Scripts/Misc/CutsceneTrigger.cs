using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject cutsceneManager;
    private ICutsceneManager cutsceneManagerI;

    void Start(){
        cutsceneManagerI = cutsceneManager.GetComponent<ICutsceneManager>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player"){
            cutsceneManagerI.PlayCutscene();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
