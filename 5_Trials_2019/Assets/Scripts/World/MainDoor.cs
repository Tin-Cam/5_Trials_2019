using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour
{
    protected Animator animator;
    //private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Open(bool isOpen)
    {
        OpenDoor(isOpen);
        SetDoorColision(isOpen);
    }

    private void OpenDoor(bool isOpen)
    {
        if (isOpen)
            animator.SetTrigger("Open");
        else
            animator.SetTrigger("Close");
    }

    private void SetDoorColision(bool isOpen)
    {

    }
}
