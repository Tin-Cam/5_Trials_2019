using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public bool showEntry;
    public GameObject targetControls;

    private AnimatorScripts animatorScripts;
    private bool triggered = false;

    void Awake() {
       animatorScripts = GetComponent<AnimatorScripts>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(triggered)
            return;
        if (other.tag != "Player")
            return;

        triggered = true;

        if(showEntry)
            ShowEntry();
        else
            ShowExit();
    }

    //Displays the target control object
    private void ShowEntry(){
        targetControls.SetActive(true);
        animatorScripts.PlayAnimation("Enter", 0);
        Destroy(this);
    }

    //Hides the target control object
    private void ShowExit(){
        StartCoroutine(ShowExitCO());
    }

    private IEnumerator ShowExitCO(){
        yield return animatorScripts.PlayWholeAnimation("Exit", 0);
        targetControls.SetActive(false);
        Destroy(this);
    }
}
