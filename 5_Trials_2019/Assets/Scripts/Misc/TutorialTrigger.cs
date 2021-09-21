using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public bool showEntry;
    public GameObject targetControls;
    public GameObject[] triggersToActivate;
    public GameObject[] triggersToDisable;

    private AnimatorScripts animatorScripts;
    private AudioManager audioManager;
    private bool triggered = false;

    void Awake() {
       animatorScripts = GetComponent<AnimatorScripts>(); 
    }

    void Start() {
        audioManager = AudioManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(triggered)
            return;
        if (other.tag != "Player")
            return;

        triggered = true;

        //Show or hide message
        if(showEntry)
            ShowEntry();
        else
            ShowExit();

        //Activate other triggers if any
        if(triggersToActivate.Length > 0){
            foreach(GameObject trigger in triggersToActivate){
                trigger.SetActive(true);
            }
        }
    }

    //Displays the target control object
    private void ShowEntry(){
        audioManager.Play("Pop_Up");
        targetControls.SetActive(true);
        animatorScripts.PlayAnimation("Enter", 0);       
        Destroy(this);
    }

    //Hides the target control object
    private void ShowExit(){
        StartCoroutine(ShowExitCO());
    }

    private IEnumerator ShowExitCO(){
        audioManager.Play("Discard");
        yield return animatorScripts.PlayWholeAnimation("Exit", 0);
        targetControls.SetActive(false);
        //Delete self and related triggers
        if(triggersToDisable.Length > 0){
            foreach(GameObject trigger in triggersToDisable){
                Destroy(trigger);
            }
        }    
        Destroy(this);
    }
}
