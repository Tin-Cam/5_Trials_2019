using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public bool showEntry;
    public GameObject targetControls;

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

        if(showEntry)
            ShowEntry();
        else
            ShowExit();
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
        Destroy(this);
    }
}
